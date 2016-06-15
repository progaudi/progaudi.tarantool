using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

using MsgPack.Light;

using tarantool_client.IProto.Data;

namespace tarantool_client
{
    public class ResponseReader :IResponseReader
    {
        private readonly Stream _networkStream;

        private readonly IRequestWriter _requestWriter;

        private readonly ILog _log;

        private readonly  MsgPackContext _msgPackContext;

        private byte[] _buffer = new byte[512];

        private int _bytesRead;

        public ResponseReader(Stream networkStream, IRequestWriter requestWriter, ILog log, MsgPackContext msgPackContext)
        {
            _networkStream = networkStream;
            _requestWriter = requestWriter;
            _log = log;

            _msgPackContext = msgPackContext;
        }

        public async Task BeginReading()
        {
            bool keepReading;
            do
            {
                var space = EnsureSpaceAndComputeBytesToRead();
                var result = await _networkStream.ReadAsync(_buffer, _bytesRead, space);
                keepReading = ProcessReadBytes(result);
            } while (keepReading);
        }

        
        private bool ProcessReadBytes(int bytesRead)
        {
            if (bytesRead <= 0)
            {
                _log.Trace("EOF");
                return false;
            }

            _bytesRead += bytesRead;
            _log.Trace("More bytes available: " + bytesRead + " (" + _bytesRead + ")");
            var offset = 0;
            var handled = ProcessBuffer(_buffer, ref offset);
            _log.Trace("Processed: " + handled);
            if (handled != 0)
            {
                // read stuff
                var remainingBytesCount = _buffer.Length - offset;
                if (remainingBytesCount > 0)
                {
                    _log.Trace("Copying remaining bytes: " + remainingBytesCount);
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

                _log.Trace(result.ToString());
                MatchResult(result);
            } while (nonEmptyResult);

            return messageCount;
        }

        private void MatchResult(byte[] result)
        {
            var header = MsgPackSerializer.Deserialize<Header>(result, _msgPackContext);

            if (header.Sync.HasValue)
            {
                _requestWriter.EndRequest(header.Sync.Value, result);
            }
            else
            {
                _log.Trace($"Found unmatched response with header: {header}");
            }
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