using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using JetBrains.Annotations;
using MessagePack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class ResponseReader : IResponseReader
    {
        private static readonly AsyncCallback EndRead = result =>
        {
            ResponseReader physical;
            if (result.CompletedSynchronously || (physical = result.AsyncState as ResponseReader) == null) return;
            if (physical.EndReading(result))
                physical.BeginReading();
        };

        private readonly IPhysicalConnection _physicalConnection;

        private readonly Dictionary<RequestId, TaskCompletionSource<(byte[] result, int bodyStart)>> _pendingRequests =
            new Dictionary<RequestId, TaskCompletionSource<(byte[] result, int bodyStart)>>();

        private readonly ReaderWriterLockSlim _pendingRequestsLock = new ReaderWriterLockSlim();

        private readonly ClientOptions _clientOptions;
        private readonly byte[] _originalBuffer;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;

        private bool _disposed;

        private readonly Thread _thread;

        private readonly ManualResetEventSlim _exitEvent;

        public ResponseReader(ClientOptions clientOptions, IPhysicalConnection physicalConnection)
        {
            _physicalConnection = physicalConnection;
            _clientOptions = clientOptions;
            _originalBuffer = _buffer = ArrayPool<byte>.Shared.Rent(1024 * 1024);
            _thread = new Thread(ReadingFunc)
            {
                IsBackground = true,
                Name = $"Tarantool :: {clientOptions.Name} :: Write"
            };
            _exitEvent = new ManualResetEventSlim(false);
        }

        private void ReadingFunc()
        {
            while (true)
            {
                if (_exitEvent.IsSet)
                {
                    return;
                }

                var result = _physicalConnection.Stream.BeginRead(_buffer, 0, _buffer.Length, EndRead, this);
                if (result.CompletedSynchronously)
                {
                    if (!EndReading(result))
                        return;
                }
            }
        }

        public bool IsConnected => !_disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _disposed = true;
            _exitEvent.Set();
            _thread.Join();
            _exitEvent.Dispose();

            ArrayPool<byte>.Shared.Return(_originalBuffer);
            var exception = new ObjectDisposedException(nameof(ResponseReader));

            lock (_pendingRequestsLock)
            {
                _clientOptions.LogWriter?.WriteLine("Cancelling all pending requests and setting faulted state...");

                foreach (var response in _pendingRequests.Values)
                {
                    response.SetException(exception);
                }

                _pendingRequests.Clear();
            }
        }

        public Task<(byte[] result, int bodyStart)> GetResponseTask(RequestId requestId)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            lock (_pendingRequestsLock)
            {
                if (_pendingRequests.ContainsKey(requestId))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }

                var tcs = new TaskCompletionSource<(byte[] result, int bodyStart)>();
                _pendingRequests.Add(requestId, tcs);

                return tcs.Task;
            }
        }

        public void BeginReading()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            try
            {
                bool keepReading;
                do
                {
                    keepReading = false;
                    var space = EnsureSpaceAndComputeBytesToRead();
                    var result = _physicalConnection.Stream.BeginRead(_buffer, _readingOffset, space, EndRead, this);
                    if (result.CompletedSynchronously)
                    {
                        keepReading = EndReading(result);
                    }
                } while (keepReading);
            }
            catch (IOException ex)
            {
                _clientOptions?.LogWriter?.WriteLine("Could not connect: " + ex.Message);
            }
        }

        private TaskCompletionSource<(byte[] result, int bodyStart)> PopResponseCompletionSource(RequestId requestId)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(ResponseReader));
            }

            TaskCompletionSource<(byte[] result, int bodyStart)> request;

            lock (_pendingRequestsLock)
            {
                if (_pendingRequests.TryGetValue(requestId, out request))
                {
                    _pendingRequests.Remove(requestId);
                }
            }

            return request;
        }

        private bool EndReading(IAsyncResult ar)
        {
            try
            {
                var bytesRead = _physicalConnection?.Stream.EndRead(ar) ?? 0;
                return ProcessReadBytes(bytesRead);
            }
            catch (Exception)
            {
                return false;
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
                    BeginReading();
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

        private void MatchResult(byte[] result)
        {
            var headerFormatter = MessagePackSerializer.DefaultResolver.GetFormatter<ResponseHeader>();
            var header = headerFormatter.Deserialize(result, 0, MessagePackSerializer.DefaultResolver, out var readSize);
            var tcs = PopResponseCompletionSource(header.RequestId);

            if (tcs == null)
            {
                if (_clientOptions.LogWriter != null)
                    LogUnMatchedResponse(result, _clientOptions.LogWriter);
                return;
            }

            _clientOptions.LogWriter?.WriteLine($"Match for request with id {header.RequestId} found.");
            if ((header.Code & CommandCodes.ErrorMask) == CommandCodes.ErrorMask)
            {
                var errorFormatter = MessagePackSerializer.DefaultResolver.GetFormatter<ErrorResponse>();
                var errorResponse = errorFormatter.Deserialize(result, readSize, MessagePackSerializer.DefaultResolver, out _);
                tcs.SetException(ExceptionHelper.TarantoolError(header, errorResponse));
            }
            else
            {
                tcs.SetResult((result, readSize));
            }
        }

        private static void LogUnMatchedResponse(byte[] result, [NotNull]ILog logWriter)
        {
            var builder = new StringBuilder("Warning: can't match request via requestId from response. Response:");
            var length = 80/3;
            for (var i = 0; i < result.Length; i++)
            {
                if (i%length == 0)
                    builder.AppendLine().Append("   ");
                else
                    builder.Append(" ");

                builder.AppendFormat("{0:X2}", result[i]);
            }

            logWriter.WriteLine(builder.ToString());
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
                var responseBuffer = ArrayPool<byte>.Shared.Rent(packetSize.Value);
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

            return (int)MessagePackBinary.ReadUInt64(_buffer, _parsingOffset, out _);
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

            _clientOptions.LogWriter?.WriteLine($"Resizing buffer from {_buffer.Length} to {_buffer.Length * 2}");

            Array.Resize(ref _buffer, _buffer.Length * 2);
            space = _buffer.Length - _readingOffset;
            return space;
        }
    }
}