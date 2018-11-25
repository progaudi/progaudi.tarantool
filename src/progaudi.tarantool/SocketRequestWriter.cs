using System.Buffers;
using System.IO;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client
{
    public sealed class SocketRequestWriter : QueueWriter
    {
        private readonly Stream _stream;

        public SocketRequestWriter(ClientOptions clientOptions, Stream stream) : base(clientOptions)
        {
            _stream = stream;
        }

        protected override void WriteRequests(uint batchSizeHint)
        {
            Request request;
            var count = 0;
            while ((request = GetRequest()) != null)
            {
                var approximateLength = Constants.PacketSizeBufferSize + request.GetApproximateLength();
                using (var bodyBuffer = MemoryPool<byte>.Shared.Rent(approximateLength))
                {
                    var span = bodyBuffer.Memory.Span;
                    var bodyLength = request.WriteTo(span.Slice(Constants.PacketSizeBufferSize));
                    LogWriter?.WriteLine($"Writing request: {bodyLength} bytes, requested {approximateLength} bytes.");
                    MsgPackSpec.WriteFixUInt32(span, (uint) bodyLength);
                    _stream.Write(span.Slice(0, Constants.PacketSizeBufferSize + bodyLength));
                    LogWriter?.WriteLine($"Wrote request {bodyLength} bytes.");
                }

                count++;
                if (batchSizeHint > 0 && count > batchSizeHint)
                {
                    break;
                }
            }

            BatchIsDone();

            _stream.Flush();
        }
    }
}
