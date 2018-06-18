using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class NetworkStreamPhysicalConnection : IPhysicalConnection
    {
        private Stream _stream;

        private Socket _socket;

        private bool _disposed;

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;

            _stream?.Dispose();
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

            _stream = new NetworkStream(_socket, true);
            options.LogWriter?.WriteLine("Socket connection established.");
        }

        public void Write(in ReadOnlySequence<byte> buffer)
        {
            CheckConnectionStatus();
            var array = buffer.ToArray();
            _stream.Write(array, 0, array.Length);
        }

        public Stream Stream => _stream;

        public async Task Flush()
        {
            CheckConnectionStatus();
            await _stream.FlushAsync().ConfigureAwait(false);
        }

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();
            return await _stream.ReadAsync(buffer, offset, count).ConfigureAwait(false);
        }

        public bool IsConnected => !_disposed && _stream != null;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckConnectionStatus()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
            }

            if (!IsConnected)
            {
                throw ExceptionHelper.NotConnected();
            }
        }
    }
}