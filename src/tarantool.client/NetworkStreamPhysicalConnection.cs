using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

using Tarantool.Client.Model;
using Tarantool.Client.Utils;

namespace Tarantool.Client
{
    internal class NetworkStreamPhysicalConnection : INetworkStreamPhysicalConnection
    {
        private Stream _stream;

        private Socket _socket;

        public void Dispose()
        {
            _stream.Dispose();
        }

        public void Connect(ConnectionOptions options)
        {
            options.LogWriter?.WriteLine("Starting socket connection...");
            _socket = new Socket(options.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(options.EndPoint);
            _stream = new NetworkStream(_socket, true);
            options.LogWriter?.WriteLine("Socket connection established.");
        }

        public async Task<int> Read(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();

            return await _stream.ReadAsync(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();
            _stream.Write(buffer, offset, count);
        }

        public async Task Flush()
        {
            CheckConnectionStatus();

            await _stream.FlushAsync();
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            CheckConnectionStatus();

            return _stream.BeginRead(buffer, offset, count, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            CheckConnectionStatus();

            return _stream.EndRead(asyncResult);
        }

        private void CheckConnectionStatus()
        {
            if (_stream == null)
            {
                throw ExceptionHelper.NotConnected();
            }
        }
    }
}