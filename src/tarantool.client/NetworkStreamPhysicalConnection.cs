using System;
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
            //var tcs = new TaskCompletionSource<int>();
            //_socket.BeginReceive(
            //    buffer,
            //    offset,
            //    count,
            //    SocketFlags.None,
            //    ar =>
            //    {
            //        var result = ((Socket) (ar.AsyncState)).EndReceive(ar);
            //        tcs.SetResult(result);
            //    },
            //    _socket);

            //return await tcs.Task;
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            return _socket.Receive(buffer, offset, count, SocketFlags.None);
            //return _stream.Read(buffer, offset, count);
        }

        public async Task WriteAsync(byte[] buffer, int offset, int count)
        {
            await _stream.WriteAsync(buffer, offset, count);
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            _socket.Send(buffer, offset, count, SocketFlags.None);
            //_stream.Write(buffer, offset, count);
        }
    }
}