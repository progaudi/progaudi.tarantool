using System;
using System.IO;
using System.Text;
using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace ProGaudi.Tarantool.Client
{
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading;

    internal class ResponseReader : IResponseReader
    {
        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        private ConcurrentDictionary<RequestId, TaskCompletionSource<MemoryStream>> _pendingRequests =
            new ConcurrentDictionary<RequestId, TaskCompletionSource<MemoryStream>>();

        private readonly ClientOptions _clientOptions;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;

        private bool _disposed;

        public ResponseReader(ClientOptions clientOptions, INetworkStreamPhysicalConnection physicalConnection)
        {
            _physicalConnection = physicalConnection;
            _clientOptions = clientOptions;
            _buffer = new byte[clientOptions.ConnectionOptions.ReadStreamBufferSize];
        }

        public Task<MemoryStream> GetResponseTask(RequestId requestId)
        {
            lock (this)
            {
                if (_pendingRequests == null)
                {
                    throw ExceptionHelper.FaultedState();
                }

                var tcs = new TaskCompletionSource<MemoryStream>();
                if (!_pendingRequests.TryAdd(requestId, tcs))
                {
                    throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
                }

                return tcs.Task;
            }
        }

        private TaskCompletionSource<MemoryStream> PopResponseCompletionSource(RequestId requestId, MemoryStream resultStream)
        {
            lock (this)
            {
                if (_pendingRequests == null)
                {
                    throw ExceptionHelper.FaultedState();
                }

                TaskCompletionSource<MemoryStream> request;

                return _pendingRequests.TryRemove(requestId, out request)
                    ? request
                    : null;
            }
        }

        public void SetFaultedState()
        {
            lock (this)
            {
                var _pendingRequestsLocal = Interlocked.Exchange(ref _pendingRequests, null);
                if (_pendingRequestsLocal == null)
                {
                    return;
                }

                _clientOptions.LogWriter?.WriteLine("Cancelling all pending requests...");

                foreach (var response in _pendingRequestsLocal.Values)
                {
                    response.SetException(new InvalidOperationException("Can't read from physical connection."));
                }

                _pendingRequestsLocal.Clear();
            }
        }

        public bool IsFaultedState => _pendingRequests == null;

        public void BeginReading()
        {
            var freeBufferSpace = EnsureSpaceAndComputeBytesToRead();

            _clientOptions.LogWriter?.WriteLine($"Begin reading from connection to buffer, bytes count: {freeBufferSpace}");

            var readingTask = _physicalConnection.ReadAsync(_buffer, _readingOffset, freeBufferSpace);
            readingTask.ContinueWith(EndReading);
        }

        private void EndReading(Task<int> readWork)
        {
            if (!_disposed)
            {
                if (readWork.IsCompleted)
                {
                    var readBytesCount = readWork.Result;
                    _clientOptions.LogWriter?.WriteLine($"End reading from connection, read bytes count: {readBytesCount}");

                    if (ProcessReadBytes(readBytesCount))
                    {
                        BeginReading();
                    }
                    else
                    {
                        this.SetFaultedState();
                    }
                }
                else
                {
                    _clientOptions.LogWriter?.WriteLine($"Connection read failed: {readWork.Exception}");
                    this.SetFaultedState();
                }
            }
            else
            {
                _clientOptions.LogWriter?.WriteLine("Attempt to end reading in disposed state... Exiting.");
            }
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
            var resultStream = new MemoryStream(result);
            var header= MsgPackSerializer.Deserialize<ResponseHeader>(resultStream, _clientOptions.MsgPackContext);
            var tcs = PopResponseCompletionSource(header.RequestId, resultStream);

            if (tcs == null)
            {
                if (_clientOptions.LogWriter != null)
                    LogUnMatchedResponse(result, _clientOptions.LogWriter);
                return;
            }

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var errorResponse = MsgPackSerializer.Deserialize<ErrorResponse>(resultStream, _clientOptions.MsgPackContext);
                tcs.SetException(ExceptionHelper.TarantoolError(header, errorResponse));
            }
            else
            {
                _clientOptions.LogWriter?.WriteLine($"Match for request with id {header.RequestId} found.");
                tcs.SetResult(resultStream);
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

        public void Dispose()
        {
            _disposed = true;
            this.SetFaultedState();
        }
    }
}