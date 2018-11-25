using System;
using System.IO;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    internal sealed class SocketResponseReader : IResponseReader
    {
        private readonly ClientOptions _clientOptions;
        private readonly Stream _stream;
        private readonly ITaskSource _taskSource;

        private byte[] _buffer;

        private int _readingOffset;

        private int _parsingOffset;
        private bool _connectionFailed;
        private readonly ILog _logWriter;

        public SocketResponseReader(ClientOptions clientOptions, Stream stream, ITaskSource taskSource)
        {
            _clientOptions = clientOptions;
            _stream = stream;
            _taskSource = taskSource;
            _buffer = new byte[clientOptions.ConnectionOptions.ReadStreamBufferSize];
            _logWriter = _clientOptions.LogWriter;
        }

        public void Start()
        {
            var freeBufferSpace = EnsureSpaceAndComputeBytesToRead();

            _logWriter?.WriteLine($"Begin reading from connection to buffer, bytes count: {freeBufferSpace}");

            var readingTask = _stream.ReadAsync(_buffer, _readingOffset, freeBufferSpace);
            readingTask.ContinueWith(EndReading);
        }

        public void Dispose()
        {
            _connectionFailed = true;
        }

        private void EndReading(Task<int> readWork)
        {
            if (_connectionFailed)
            {
                _logWriter?.WriteLine("Attempt to end reading in disposed state... Exiting.");
                return;
            }

            if (readWork.Status == TaskStatus.RanToCompletion)
            {
                var readBytesCount = readWork.Result;
                _logWriter?.WriteLine($"End reading from connection, read bytes count: {readBytesCount}");

                if (ProcessReadBytes(readBytesCount))
                {
                    Start();
                    return;
                }
            }

            _logWriter?.WriteLine($"Connection read failed: {readWork.Exception}");
            _connectionFailed = true;
        }

        private bool ProcessReadBytes(int readBytesCount)
        {
            if (readBytesCount <= 0)
            {
                _logWriter?.WriteLine("EOF");
                return false;
            }

            _readingOffset += readBytesCount;
            var parsedResponsesCount = TryParseResponses();
            _logWriter?.WriteLine("Processed: " + parsedResponsesCount);
            if (!AllBytesProcessed())
            {
                CopyRemainingBytesToBufferBegin();
            }

            return !_connectionFailed;
        }

        private void CopyRemainingBytesToBufferBegin()
        {
            var remainingBytesCount = _readingOffset - _parsingOffset;
            if (remainingBytesCount > 0)
            {
                _logWriter?.WriteLine("Copying remaining bytes: " + remainingBytesCount);
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
                _taskSource.MatchResult(response);
            } while (nonEmptyResult);

            return messageCount;
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
                _logWriter?.WriteLine(
                    $"Can't read packet length, has less than {Constants.PacketSizeBufferSize} bytes.");
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

            _logWriter?.WriteLine($"Packet  with length {packetSize} is not completely read.");

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
            var packetSize =
                (int) MsgPackSerializer.Deserialize<ulong>(headerSizeBuffer, _clientOptions.MsgPackContext);

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