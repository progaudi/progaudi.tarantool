using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public sealed class SocketPhysicalConnection : IPhysicalConnection
    {
        private Socket _socket;

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            _socket?.Dispose();
        }

        public async Task Connect(ClientOptions options)
        {
            options.LogWriter?.WriteLine("Starting socket connection...");
            var singleNode = options.ConnectionOptions.Nodes.Single();

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true,
                //Blocking = false
            };
            await _socket.ConnectAsync(singleNode.Uri.Host, singleNode.Uri.Port).ConfigureAwait(false);

            options.LogWriter?.WriteLine("Socket connection established.");
        }

        public Task Flush()
        {
            throw new NotImplementedException();
        }

        public bool IsConnected { get; }
        public Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }

        public void Write(in ArraySegment<byte> buffer)
        {
            throw new NotImplementedException();
        }

        public Stream Stream { get; }

        public void Write(byte[] buffer, int offset, int count)
        {
            throw new NotImplementedException();
        }
    }
}