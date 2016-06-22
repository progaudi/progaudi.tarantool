using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

using Tarantool.Client.Utils;

namespace Tarantool.Client
{
    public class NetworkStreamPhysicalConnection
    {
        private Stream _stream;

        private Socket _socket;
        
        public void Dispose()
        {
            _stream.Dispose();
        }

        public void Connect(ConnectionOptions options)
        {
            _socket = new Socket(options.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(options.EndPoint);
            _stream = new NetworkStream(_socket, true);
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();

            return await _stream.ReadAsync(buffer, offset, count);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();

            await _stream.WriteAsync(buffer, offset, count);
        }

        public async Task FlushAsync()
        {
            CheckConnectionStatus();

            await _stream.FlushAsync();
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            CheckConnectionStatus();

            return _socket.BeginReceive(buffer, offset, count, SocketFlags.None, callback, state);
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