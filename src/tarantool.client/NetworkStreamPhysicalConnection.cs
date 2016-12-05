using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

#if PROGAUDI_NETCORE
using System.Net;
#endif

namespace ProGaudi.Tarantool.Client
{
    using System.Runtime.InteropServices;
    using System.Threading;

    internal class NetworkStreamPhysicalConnection : INetworkStreamPhysicalConnection
    {
        private Stream _stream;

        private Socket _socket;

        private bool _disposed;

        public void Dispose()
        {
            _disposed = true;
            Interlocked.Exchange(ref _stream, null)?.Dispose();
            Interlocked.Exchange(ref _socket, null)?.Dispose();
        }

        public async Task Connect(ClientOptions options)
        {
            options.LogWriter?.WriteLine("Starting socket connection...");
            var singleNode = options.ConnectionOptions.Nodes.Single();

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            await ConnectAsync(_socket, singleNode.Uri.Host, singleNode.Uri.Port);
            SetKeepAlive(true, 1000, 100);
            _stream = new NetworkStream(_socket, true);
            options.LogWriter?.WriteLine("Socket connection established.");
        }

        private void SetKeepAlive(bool on, uint keepAliveTime, uint keepAliveInterval)
        {
            int size = Marshal.SizeOf(new uint());

            var inOptionValues = new byte[size * 3];

            BitConverter.GetBytes((uint)(on ? 1 : 0)).CopyTo(inOptionValues, 0);
            BitConverter.GetBytes(keepAliveTime).CopyTo(inOptionValues, size);
            BitConverter.GetBytes(keepAliveInterval).CopyTo(inOptionValues, size * 2);

            _socket.IOControl(IOControlCode.KeepAliveValues, inOptionValues, null);
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

        public async Task<int> ReadAsync(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();
            return await _stream.ReadAsync(buffer, offset, count);
        }

#if PROGAUDI_NETCORE
        /// https://github.com/mongodb/mongo-csharp-driver/commit/9c2097f349d5096a04ea81b0c9ceb60c7e1acee4
        private static async Task ConnectAsync(Socket socket, string host, int port)
        {
            var resolved = await Dns.GetHostAddressesAsync(host);
            for (int i = 0; i < resolved.Length; i++)
            {
                try
                {
                    await socket.ConnectAsync(resolved[i], port);
                    return;
                }
                catch
                {
                    // if we have tried all of them and still failed,
                    // then blow up.
                    if (i == resolved.Length - 1)
                    {
                        throw;
                    }
                }
            }

            // we should never get here...
            throw new InvalidOperationException("Unabled to resolve endpoint.");
        }
#else
        /// Stolen from corefx github
        private static Task ConnectAsync(Socket socket, string host, int port)
        {
            return Task.Factory.FromAsync(
                (targetHost, targetPort, callback, state) => ((Socket)state).BeginConnect(targetHost, targetPort, callback, state),
                asyncResult => ((Socket)asyncResult.AsyncState).EndConnect(asyncResult),
                host,
                port,
                socket);
        }
#endif

        public bool IsConnected()
        {
            try
            {
                return !(_socket.Poll(1, SelectMode.SelectRead) && _socket.Available == 0);
            }
            catch (SocketException) { return false; }
        }

        private void CheckConnectionStatus()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
            }

            if (!IsConnected() || _stream == null)
            {
                throw ExceptionHelper.NotConnected();
            }
        }
    }
}