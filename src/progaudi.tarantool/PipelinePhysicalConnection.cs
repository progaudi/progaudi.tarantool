using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Pipelines.Sockets.Unofficial;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Utils;

#if PROGAUDI_NETCORE
using System.Net;
#endif

namespace ProGaudi.Tarantool.Client
{
//    internal class PipelinePhysicalConnection : IPhysicalConnection
//    {
//        private bool _disposed;
//        private SocketConnection _pipeline;
//
//        public void Dispose()
//        {
//            if (_disposed)
//            {
//                return;
//            }
//
//            _disposed = true;
//            _pipeline.Dispose();
//        }
//
//        public async Task Connect(ClientOptions options)
//        {
//            if (! options.ConnectionOptions.Nodes.Any()) 
//                throw new ClientSetupException("There are zero configured nodes, you should provide one");
//
//            options.LogWriter?.WriteLine("Starting socket connection...");
//            var singleNode = options.ConnectionOptions.Nodes.Single();
//
//            _pipeline = await SocketConnection.ConnectAsync(new DnsEndPoint(singleNode.Host, singleNode.Port));
//            options.LogWriter?.WriteLine("Socket connection established.");
//        }
//
//        public void Write(Request request)
//        {
//            CheckConnectionStatus();
//            var approximateLength = Constants.PacketSizeBufferSize + request.GetApproximateLength();
//            var span = _pipeline.Output.GetSpan(approximateLength);
//            var bodyLength = request.WriteTo(span.Slice(Constants.PacketSizeBufferSize));
//            MsgPackSpec.WriteFixUInt32(span, (uint) bodyLength);
//            _pipeline.Output.Advance(bodyLength);
//        }
//
//        public async Task Flush()
//        {
//            CheckConnectionStatus();
//            var task = _pipeline.Output.FlushAsync();
//            if (!task.IsCompleted)
//                await task;
//        }
//
//        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
//        {
//            CheckConnectionStatus();
//            return 0;
//        }
//
//        public bool IsConnected => !_disposed && _pipeline != null;
//
//        private void CheckConnectionStatus()
//        {
//            if (_disposed)
//            {
//                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
//            }
//
//            if (!IsConnected)
//            {
//                throw ExceptionHelper.NotConnected();
//            }
//        }
//    }
}
