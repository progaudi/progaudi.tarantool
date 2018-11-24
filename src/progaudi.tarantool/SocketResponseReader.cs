using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class SocketResponseReader : IResponseReader
    {
        private readonly Dictionary<RequestId, TaskCompletionSource<ReadOnlyMemory<byte>>> _pendingRequests =
            new Dictionary<RequestId, TaskCompletionSource<ReadOnlyMemory<byte>>>();

        private readonly ReaderWriterLockSlim _pendingRequestsLock = new ReaderWriterLockSlim();

        private readonly ClientOptions _clientOptions;
        private readonly Stream _stream;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;

        private bool _disposed;
        private readonly IMsgPackParser<ResponseHeader> _headerParser;
        private readonly IMsgPackParser<ErrorResponse> _errorParser;

        public SocketResponseReader(ClientOptions clientOptions, Stream stream)
        {
            _clientOptions = clientOptions;
            _stream = stream;
            _buffer = new byte[clientOptions.ConnectionOptions.ReadStreamBufferSize];
            _headerParser = _clientOptions.MsgPackContext.GetRequiredParser<ResponseHeader>();
            _errorParser = _clientOptions.MsgPackContext.GetRequiredParser<ErrorResponse>();
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            try
            {
                _pendingRequestsLock.EnterWriteLock();

                _clientOptions.LogWriter?.WriteLine("Cancelling all pending requests and setting faulted state...");

                foreach (var response in _pendingRequests.Values)
                {
                    response.SetException(new ObjectDisposedException(nameof(SocketResponseReader)));
                }

                _pendingRequests.Clear();
            }
            finally
            {
                _pendingRequestsLock.ExitWriteLock();
            }
        }

        public Task<ReadOnlyMemory<byte>> GetResponseTask(RequestId requestId)
        {
            try
            {
                _pendingRequestsLock.EnterWriteLock();

                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(SocketResponseReader));
                }

                if (_pendingRequests.ContainsKey(requestId))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }

                var tcs = new TaskCompletionSource<ReadOnlyMemory<byte>>();
                _pendingRequests.Add(requestId, tcs);

                return tcs.Task;
            }
            finally
            {
                _pendingRequestsLock.ExitWriteLock();
            }
        }

        public void Start()
        {
            var freeBufferSpace = EnsureSpaceAndComputeBytesToRead();

            _clientOptions.LogWriter?.WriteLine($"Begin reading from connection to buffer, bytes count: {freeBufferSpace}");

            var readingTask = _stream.ReadAsync(_buffer, _readingOffset, freeBufferSpace);
            readingTask.ContinueWith(EndReading);
        }

        private TaskCompletionSource<ReadOnlyMemory<byte>> PopResponseCompletionSource(RequestId requestId)
        {
            try
            {
                _pendingRequestsLock.EnterWriteLock();

                if (_disposed)
                {
                    throw new ObjectDisposedException(nameof(SocketResponseReader));
                }

                if (_pendingRequests.TryGetValue(requestId, out var request))
                {
                    _pendingRequests.Remove(requestId);
                }

                return request;
            }
            finally
            {
                _pendingRequestsLock.ExitWriteLock();
            }
        }

        private void EndReading(Task<int> readWork)
        {
            if (_disposed)
            {
                _clientOptions.LogWriter?.WriteLine("Attempt to end reading in disposed state... Exiting.");
                return;
            }

            if (readWork.Status == TaskStatus.RanToCompletion)
            {
                var readBytesCount = readWork.Result;
                _clientOptions.LogWriter?.WriteLine($"End reading from connection, read bytes count: {readBytesCount}");

                if (ProcessReadBytes(readBytesCount))
                {
                    Start();
                    return;
                }
            }

            _clientOptions.LogWriter?.WriteLine($"Connection read failed: {readWork.Exception}");
            Dispose();
        }

        private bool ProcessReadBytes(int readBytesCount)
        {
            if (readBytesCount <= 0)
            {
                _clientOptions.LogWriter?.WriteLine("EOF");
                return false;
            }
            _readingOffset += readBytesCount;
            var parsedResponsesCount = TryParseResponses();
            _clientOptions.LogWriter?.WriteLine("Processed: " + parsedResponsesCount);
            if (!AllBytesProcessed())
            {
                CopyRemainingBytesToBufferBegin();
            }

            return !_disposed;
        }

        private void CopyRemainingBytesToBufferBegin()
        {
            var remainingBytesCount = _readingOffset - _parsingOffset;
            if (remainingBytesCount > 0)
            {
                _clientOptions.LogWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
                Buffer.BlockCopy(_buffer, _parsingOffset, _buffer, 0, remainingBytesCount);
                Array.Clear(_buffer, remainingBytesCount, _buffer.Length - remainingBytesCount);
            }

            _readingOffset = remainingBytesCount;
            _parsingOffset = 0;
        }

        private bool AllBytesProcessed()
        {
            return _parsingOffset >= _readingOffset;
        }

        private int TryParseResponses()
        {
            var messageCount = 0;
            bool nonEmptyResult;
            do
            {
                var response = TryParseResponse();
                nonEmptyResult = response != null;
                if (!nonEmptyResult)
                {
                    continue;
                }

                messageCount++;
                MatchResult(response);
            } while (nonEmptyResult);
            return messageCount;
        }

        private void MatchResult(ReadOnlyMemory<byte> result)
        {
            var readSize = 0;
            var header = _headerParser.Parse(result.Span, out var temp);
            readSize += temp;
            var tcs = PopResponseCompletionSource(header.Id);

            if (tcs == null)
            {
                _clientOptions.LogWriter?.WriteLine($"Warning: can't match request via requestId from response. Response:\n{ByteUtils.ToReadableString(result.Span)}");

                return;
            }

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var errorResponse = _errorParser.Parse(result.Slice(readSize).Span, out _);
                tcs.SetException(ExceptionHelper.TarantoolError(header, errorResponse));
            }
            else
            {
                _clientOptions.LogWriter?.WriteLine($"Match for request with id {header.Id} found.");
                tcs.SetResult(result.Slice(readSize));
            }
        }

        private byte[] TryParseResponse()
        {
            if (AllBytesProcessed())
            {
                return null;
            }

            var packetSize = GetPacketSize();

            if (!packetSize.HasValue)
            {
                _clientOptions.LogWriter?.WriteLine($"Can't read packet length, has less than {Constants.PacketSizeBufferSize} bytes.");
                return null;
            }

            if (PacketCompletelyRead(packetSize.Value))
            {
                _parsingOffset += Constants.PacketSizeBufferSize;
                var responseBuffer = new byte[packetSize.Value];
                Array.Copy(_buffer, _parsingOffset, responseBuffer, 0, packetSize.Value);
                _parsingOffset += packetSize.Value;


                return responseBuffer;
            }

            _clientOptions.LogWriter?.WriteLine($"Packet  with length {packetSize} is not completely read.");

            return null;
        }

        private int? GetPacketSize()
        {
            if (_readingOffset - _parsingOffset < Constants.PacketSizeBufferSize)
            {
                return null;
            }

            var headerSizeBuffer = new byte[Constants.PacketSizeBufferSize];
            Array.Copy(_buffer, _parsingOffset, headerSizeBuffer, 0, Constants.PacketSizeBufferSize);
            var packetSize = (int)MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer, _clientOptions.MsgPackContext);

            return packetSize;
        }

        private bool PacketCompletelyRead(int packetSize)
        {
            return packetSize <= _readingOffset - _parsingOffset - Constants.PacketSizeBufferSize;
        }

        private int EnsureSpaceAndComputeBytesToRead()
        {
            var space = _buffer.Length - _readingOffset;
            if (space != 0)
            {
                return space;
            }

            Array.Resize(ref _buffer, _buffer.Length * 2);
            space = _buffer.Length - _readingOffset;
            return space;
        }
    }
}