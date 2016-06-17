using System;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;

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
            var freeBufferSpace= EnsureSpaceAndComputeBytesToRead();

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
            var handled = ProcessBuffer(_buffer, ref offset);
            _connectionOptions.LogWriter?.WriteLine("Processed: " + handled);
            if (handled != 0)
            {
                // read stuff
                var remainingBytesCount = _buffer.Length - offset;
                if (remainingBytesCount > 0)
                {
                    _connectionOptions.LogWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
                    //  if anything was left over, we need to copy it to
                    // the start of the buffer so it can be used next time
                    Buffer.BlockCopy(_buffer, offset, _buffer, 0, remainingBytesCount);
                }
                _bytesRead = remainingBytesCount;
            }
            return true;
        }

        private int ProcessBuffer(byte[] underlying, ref int offset)
        {
            var messageCount = 0;
            bool nonEmptyResult;
            do
            {
                int tmpOffset = offset;
                // we want TryParseResult to be able to mess with these without consequence
                var result = TryParseResult(underlying, ref tmpOffset);
                nonEmptyResult = result != null && result.Length > 0;
                if (!nonEmptyResult)
                {
                    continue;
                }

                messageCount++;
                // entire message: update the external counters
                offset = tmpOffset;

                _connectionOptions.LogWriter?.WriteLine(result.ToString());
                MatchResult(result);
            } while (nonEmptyResult);

            return messageCount;
        }

        private void MatchResult(byte[] result)
        {
            var header = MsgPackSerializer.Deserialize<ResponseHeader>(result, _connectionOptions.MsgPackContext);

            var tcs = _logicalConnection.PopResponseCompletionSource(header.RequestId);
            tcs.SetResult(result);
        }

        private byte[] TryParseResult(byte[] buffer, ref int offset)
        {
            const int headerSizeBufferSize = 5;
            var headerSizeBuffer = new byte[headerSizeBufferSize];
            Array.Copy(buffer, offset, headerSizeBuffer, 0, headerSizeBufferSize);
            offset += headerSizeBufferSize;

            var headerSize = (int)MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer);

            var responseBuffer = new byte[headerSize];
            Array.Copy(buffer, offset, responseBuffer, 0, headerSize);
            offset += headerSize;

            return responseBuffer;
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