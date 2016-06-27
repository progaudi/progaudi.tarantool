using System;
using System.IO;

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

        private int _bufferOffset;

        private int? _currentPacketSize;

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

            _physicalConnection.BeginRead(_buffer, _bufferOffset, freeBufferSpace, EndReading, this);
        }

        private void EndReading(IAsyncResult ar)
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

            var offset = _bufferOffset;
            _bufferOffset += readBytesCount;

            _connectionOptions.LogWriter?.WriteLine("More bytes available: " + readBytesCount + " (" + _bufferOffset + ")");

            var handledMessagesCount = ReadMessages(ref offset, readBytesCount);
            _connectionOptions.LogWriter?.WriteLine("Processed: " + handledMessagesCount);

            if (handledMessagesCount == 0)
            {
                return true;
            }

            if (!UnprocessedBytesLeft(offset, readBytesCount))
            {
                _bufferOffset = 0;
                return true;
            }

            ProcessRemainingBytes(offset);

            return true;
        }

        private void ProcessRemainingBytes(int offset)
        {
            var remainingBytesCount = _buffer.Length - offset;
            if (remainingBytesCount > 0)
            {
                _connectionOptions.LogWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
                //  if anything was left over, we need to copy it to
                // the start of the buffer so it can be used next time
                Buffer.BlockCopy(_buffer, offset, _buffer, 0, remainingBytesCount);
            }
            _bufferOffset = remainingBytesCount;
        }

        private bool UnprocessedBytesLeft(int offset, int bytesRead)
        {
            return offset != bytesRead;
        }

        private int ReadMessages(ref int offset, int bytesRead)
        {
            var messageCount = 0;
            bool nonEmptyResult;
            do
            {
                var response = TryReadResponse(ref offset, bytesRead);
                nonEmptyResult = response != null && response.Length > 0;
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

            var header = MsgPackSerializer.Deserialize<ResponseHeader>(resultStream, _connectionOptions.MsgPackContext);

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
        
        private byte[] TryReadResponse(ref int offset, int bytesRead)
        {
            if (!UnprocessedBytesLeft(offset, bytesRead))
                return null;

            var packetSize = GetPacketSize(ref offset);

            if (PacketCompletelyRead(packetSize, offset))
            {
                var responseBuffer = new byte[packetSize];
                Array.Copy(_buffer, offset, responseBuffer, 0, packetSize);
                offset += packetSize;

                _connectionOptions.LogWriter?.WriteLine($"Packet with length {packetSize} successfully parsed.");

                _currentPacketSize = null;

                return responseBuffer;
            }
            else
            {
                _connectionOptions.LogWriter?.WriteLine($"Packet  with length {packetSize} is not completely read.");
            }

            _currentPacketSize = packetSize;
            return null;
        }

        private int GetPacketSize(ref int offset)
        {
            int packetSize;

            if (!_currentPacketSize.HasValue)
            {
                var headerSizeBuffer = new byte[Constants.PacketSizeBufferSize];
                Array.Copy(_buffer, offset, headerSizeBuffer, 0, Constants.PacketSizeBufferSize);
                packetSize = (int) MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer, _connectionOptions.MsgPackContext);
            }
            else
            {
                packetSize = _currentPacketSize.Value;
            }

            offset += Constants.PacketSizeBufferSize;

            return packetSize;
        }

        private bool PacketCompletelyRead(int packetSize, int offset)
        {
            return packetSize <= _bufferOffset - offset;
        }

        private int EnsureSpaceAndComputeBytesToRead()
        {
            var space = _buffer.Length - _bufferOffset;
            if (space != 0)
            {
                return space;
            }

            Array.Resize(ref _buffer, _buffer.Length * 2);
            space = _buffer.Length - _bufferOffset;
            return space;
        }

    }
}