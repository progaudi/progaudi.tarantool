using System;
using System.IO;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public class ResponseReader : IResponseReader
    {
        private readonly IPhysicalConnection _physicalConnection;

        private readonly ILogicalConnection _logicalConnection;

        private readonly ConnectionOptions _connectionOptions;

        private byte[] _buffer = new byte[512];

        private int _bytesRead;

        public ResponseReader(ILogicalConnection logicalConnection, ConnectionOptions connectionOptions)
        {
            _physicalConnection = connectionOptions.PhysicalConnection;
            _logicalConnection = logicalConnection;
            _connectionOptions = connectionOptions;
        }

        public void BeginReading()
        {
            var freeBufferSpace = EnsureSpaceAndComputeBytesToRead();

            _physicalConnection.BeginRead(_buffer, _bytesRead, freeBufferSpace, EndReading, this);
        }

        private void EndReading(IAsyncResult ar)
        {
            var bytesRead = _physicalConnection.EndRead(ar);
            if (ProcessReadBytes(bytesRead))
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
            var responses = _logicalConnection.PopAllResponseCompletionSources();
            foreach (var response in responses)
            {
                response.SetException(new InvalidOperationException("Can't read from physical connection."));
            }
        }

        private bool ProcessReadBytes(int bytesRead)
        {
            if (bytesRead <= 0)
            {
                _connectionOptions.LogWriter?.WriteLine("EOF");
                return false;
            }

            _bytesRead += bytesRead;
            _connectionOptions.LogWriter?.WriteLine("More bytes available: " + bytesRead + " (" + _bytesRead + ")");
            var offset = 0;
            var handled = ProcessBuffer(ref offset);
            _connectionOptions.LogWriter?.WriteLine("Processed: " + handled);

            if (handled == 0)
            {
                return true;
            }

            if (!UnprocessedBytesLeft(offset, bytesRead))
            {
                _bytesRead = 0;
                return true;
            }

            var remainingBytesCount = _buffer.Length - offset;
            if (remainingBytesCount > 0)
            {
                _connectionOptions.LogWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
                //  if anything was left over, we need to copy it to
                // the start of the buffer so it can be used next time
                Buffer.BlockCopy(_buffer, offset, _buffer, 0, remainingBytesCount);
            }
            _bytesRead = remainingBytesCount;
            return true;
        }

        private bool UnprocessedBytesLeft(int offset, int bytesRead)
        {
            return offset != bytesRead;
        }

        private int ProcessBuffer(ref int offset)
        {
            var messageCount = 0;
            bool nonEmptyResult;
            do
            {
                int tmpOffset = offset;
                // we want TryParseResult to be able to mess with these without consequence
                var result = TryParseResult(ref tmpOffset);
                nonEmptyResult = result != null && result.Length > 0;
                if (!nonEmptyResult)
                {
                    continue;
                }

                messageCount++;
                // entire message: update the external counters
                offset = tmpOffset;

                MatchResult(result);
            } while (nonEmptyResult);

            return messageCount;
        }

        private void MatchResult(byte[] result)
        {
            var resultStream = new MemoryStream(result);

            var header = MsgPackSerializer.Deserialize<ResponseHeader>(resultStream, _connectionOptions.MsgPackContext);

            var tcs = _logicalConnection.PopResponseCompletionSource(header.RequestId);

            if ((header.Code & CommandCode.ErrorMask) == CommandCode.ErrorMask)
            {
                var errorResponse = MsgPackSerializer.Deserialize<ErrorResponsePacket>(resultStream, _connectionOptions.MsgPackContext);
                tcs.SetException(new ArgumentException($"Tarantool returns an error with code: 0x{header.Code:X}  and message: {errorResponse.ErrorMessage}"));
            }
            else
            {
                _connectionOptions.LogWriter?.WriteLine($"Match for request with id {header.RequestId} found.");
                tcs.SetResult(resultStream);
            } 
        }

        private byte[] TryParseResult(ref int offset)
        {
            const int headerSizeBufferSize = 5;
            var headerSizeBuffer = new byte[headerSizeBufferSize];
            Array.Copy(_buffer, offset, headerSizeBuffer, 0, headerSizeBufferSize);
            offset += headerSizeBufferSize;

            //TODO don't read packet size each time
            var packetSize = (int)MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer);

            if (PacketCompletelyRead(packetSize, offset))
            {
                var responseBuffer = new byte[packetSize];
                Array.Copy(_buffer, offset, responseBuffer, 0, packetSize);
                offset += packetSize;

                return responseBuffer;
            }
            else
            {
                return null;
            }
        }

        private bool PacketCompletelyRead(int packetSize, int offset)
        {
            return packetSize == _bytesRead - offset;
        }

        private int EnsureSpaceAndComputeBytesToRead()
        {
            int space = _buffer.Length - _bytesRead;
            if (space == 0)
            {
                Array.Resize(ref _buffer, _buffer.Length * 2);
                space = _buffer.Length - _bytesRead;
            }
            return space;
        }

    }
}