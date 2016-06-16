using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    public class NetworkStreamPhysicalConnection : IPhysicalConnection
    {
        private Stream _stream;
        private readonly Socket _socket;

        public NetworkStreamPhysicalConnection()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        }

        public void Dispose()
        {
            _stream.Dispose();
        }

        public void Connect(ConnectionOptions options)
        {
            _socket.Connect(options.EndPoint);
            _stream = new BufferedStream(new NetworkStream(_socket), options.StreamBufferSize);
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            return await _stream.ReadAsync(buffer, offset, count);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await _stream.WriteAsync(buffer, offset, count);
        }
    }
}