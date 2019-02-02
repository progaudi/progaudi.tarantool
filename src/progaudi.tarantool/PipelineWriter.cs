using System;
using System.IO.Pipelines;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client
{
    internal sealed class PipelineWriter : QueueWriter
    {
        private readonly PipeWriter _pipeWriter;

        public PipelineWriter(PipeWriter pipeWriter, ClientOptions options)
            : base(options)
        {
            _pipeWriter = pipeWriter;
        }
        
        protected override void WriteRequests(uint batchSizeHint)
        {
            Request request;
            var count = 0;
            var buffer = Span<byte>.Empty;
            while ((request = GetRequest()) != null)
            {
                var approximateLength = Constants.PacketSizeBufferSize + request.GetApproximateLength();
                if (buffer.Length < approximateLength)
                {
                    buffer = _pipeWriter.GetSpan(approximateLength);
                }

                var bodyLength = request.WriteTo(buffer.Slice(Constants.PacketSizeBufferSize));
                LogWriter?.WriteLine($"Writing request: {bodyLength} bytes, requested {approximateLength} bytes.");
                bodyLength += MsgPackSpec.WriteFixUInt32(buffer, (uint) bodyLength);
                LogWriter?.WriteLine($"Wrote request {bodyLength} bytes.");
                _pipeWriter.Advance(bodyLength);
                buffer = buffer.Slice(bodyLength);

                count++;
                if (batchSizeHint > 0 && count > batchSizeHint)
                {
                    break;
                }
            }

            FlushSync();
            BatchIsDone();

            void FlushSync()
            {
                var flushTask = _pipeWriter.FlushAsync();
                if (!flushTask.IsCompletedSuccessfully) flushTask.AsTask().Wait();
            }
        }
    }
}