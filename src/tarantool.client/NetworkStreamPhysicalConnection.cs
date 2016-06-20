using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    public class NetworkStreamPhysicalConnection : IPhysicalConnection
    {
        private Stream _stream;

        private Socket _socket;

        public NetworkStreamPhysicalConnection()
        {
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public void Connect(ConnectionOptions options)
        {
            _socket = new Socket(options.EndPoint.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
            _socket.Connect(options.EndPoint);
            //_stream = new BufferedStream(new NetworkStream(_socket), options.StreamBufferSize);
            _stream = new NetworkStream(_socket, true);
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return await _stream.ReadAsync(buffer, offset, count);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await _stream.WriteAsync(buffer, offset, count);
        }

        public async Task FlushAsync()
        {
            await _stream.FlushAsync();
        }

        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _socket.BeginReceive(buffer, offset, count, SocketFlags.None, callback, state);
        }

        public int EndRead(IAsyncResult asyncResult)
        {
            return _stream.EndRead(asyncResult);
        }
    }
}