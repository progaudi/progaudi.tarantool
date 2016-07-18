using System;
using System.IO;
using System.Linq;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Headers;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

namespace Tarantool.Client
{
    internal class ResponseReader : IResponseReader
    {
        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        private readonly ILogicalConnection _logicalConnection;

        private readonly ConnectionOptions _connectionOptions;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;

        private bool _disposed = false;

        public ResponseReader(ILogicalConnection logicalConnection, ConnectionOptions connectionOptions, INetworkStreamPhysicalConnection physicalConnection)
        {
            _physicalConnection = physicalConnection;
            _logicalConnection = logicalConnection;
            _connectionOptions = connectionOptions;
            _buffer = new byte[connectionOptions.StreamBufferSize];
        }

        public void BeginReading()
        {
            var freeBufferSpace = EnsureSpaceAndComputeBytesToRead();

            _connectionOptions.LogWriter?.WriteLine($"Begin reading from connection to buffer, bytes count: {freeBufferSpace}");

            _physicalConnection.BeginRead(_buffer, _readingOffset, freeBufferSpace, EndReading, this);
        }

        private void EndReading(IAsyncResult ar)
        {
            if (!_disposed)
            {
                var readBytesCount = _physicalConnection.EndRead(ar);
                _connectionOptions.LogWriter?.WriteLine($"End reading from connection, read bytes count: {readBytesCount}");

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
                _connectionOptions.LogWriter?.WriteLine($"Attempt to end reading in disposed state... Exiting.");
            }
        }

        private void CancelAllPendingRequests()
        {
            _connectionOptions.LogWriter?.WriteLine("Cancelling all pending requests...");
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
                _connectionOptions.LogWriter?.WriteLine("EOF");
                return false;
            }
            _readingOffset += readBytesCount;
            var parsedResponsesCount = TryParseResponses();
            _connectionOptions.LogWriter?.WriteLine("Processed: " + parsedResponsesCount);
            if (!AllBytesProcessed())
            {
                CopyRemainingBytesToBufferBegin();
            }

            return true;
        }

        private void CopyRemainingBytesToBufferBegin()
        {
            var remainingBytesCount = _readingOffset - _parsingOffset;
            if (remainingBytesCount > 0)
            {
                _connectionOptions.LogWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
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
            var header= MsgPackSerializer.Deserialize<ResponseHeader>(resultStream, _connectionOptions.MsgPackContext);
            var tcs = _logicalConnection.PopResponseCompletionSource(header.RequestId, resultStream);

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var errorResponse = MsgPackSerializer.Deserialize<ErrorResponse>(resultStream, _connectionOptions.MsgPackContext);
                tcs.SetException(ExceptionHelper.TarantoolError(header, errorResponse));
            }
            else
            {
                _connectionOptions.LogWriter?.WriteLine($"Match for request with id {header.RequestId} found.");
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
                _connectionOptions.LogWriter?.WriteLine($"Can't read packet length, has less than {Constants.PacketSizeBufferSize} bytes.");
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

            _connectionOptions.LogWriter?.WriteLine($"Packet  with length {packetSize} is not completely read.");

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
            var packetSize = (int)MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer, _connectionOptions.MsgPackContext);

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