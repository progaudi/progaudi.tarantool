using System;
using System.CodeDom;
using System.IO;
using System.Linq;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client
{
    internal class ResponseReader : IResponseReader
    {
        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        private readonly ILogicalConnection _logicalConnection;

        private readonly ClientOptions _clientOptions;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;

        private bool _disposed = false;

        public ResponseReader(ILogicalConnection logicalConnection, ClientOptions clientOptions, INetworkStreamPhysicalConnection physicalConnection)
        {
            _physicalConnection = physicalConnection;
            _logicalConnection = logicalConnection;
            _clientOptions = clientOptions;
            _buffer = new byte[clientOptions.ConnectionOptions.ReadStreamBufferSize];
        }

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
                var readBytesCount = readWork.Result;
                _clientOptions.LogWriter?.WriteLine($"End reading from connection, read bytes count: {readBytesCount}");

                if (ProcessReadBytes(readBytesCount))
                {
                    BeginReading();
                }
                else
                {
                    CancelAllPendingRequests();
                }
            }
            else
            {
                _clientOptions.LogWriter?.WriteLine("Attempt to end reading in disposed state... Exiting.");
            }
        }

        private void CancelAllPendingRequests()
        {
            _clientOptions.LogWriter?.WriteLine("Cancelling all pending requests...");
            var responses = _logicalConnection.PopAllResponseCompletionSources();
            foreach (var response in responses)
            {
                response.SetException(new InvalidOperationException("Can't read from physical connection."));
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
            var tcs = _logicalConnection.PopResponseCompletionSource(header.RequestId, resultStream);

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
        }
    }
}