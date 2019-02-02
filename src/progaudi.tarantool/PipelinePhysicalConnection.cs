using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Utils;

#if PROGAUDI_NETCORE
using System.Net;
#endif

namespace ProGaudi.Tarantool.Client
{
    internal class PipelinePhysicalConnection : PhysicalConnection
    {
        private SocketConnection _pipeline;

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (!disposing) return;
            _pipeline?.Dispose();
        }

        protected override async Task<ReadOnlyMemory<byte>> ConnectAndReadGreeting(ClientOptions options, TarantoolNode singleNode)
        {
            options.LogWriter?.WriteLine("Starting socket pipeline connection...");

            _pipeline = await SocketConnection.ConnectAsync(new DnsEndPoint(singleNode.Host, singleNode.Port));
            options.LogWriter?.WriteLine("Socket connection pipeline established.");

            var task = _pipeline.Input.ReadAsync();
            var readResult = task.IsCompletedSuccessfully ? task.Result : await task;
            var result = readResult.Buffer.IsSingleSegment ? readResult.Buffer.First : readResult.Buffer.ToArray();
            _pipeline.Input.AdvanceTo(readResult.Buffer.End);

            Writer = new PipelineWriter(_pipeline.Output, options);
            Reader = new PipelineReader(_pipeline.Input, TaskSource, options);
            
            return result;
        }

        public override bool IsConnected => _pipeline != null && base.IsConnected;
    }
}
