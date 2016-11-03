using System;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

#if PROGAUDI_NETCORE
using System.Net;
#endif

namespace ProGaudi.Tarantool.Client
{
    internal class NetworkStreamPhysicalConnection : INetworkStreamPhysicalConnection
    {
        private Stream _stream;

        private Socket _socket;

        private bool _disposed;

        public void Dispose()
        {
            _disposed = true;
            _stream?.Dispose();
        }

        public async Task Connect(ClientOptions options)
        {
            options.LogWriter?.WriteLine("Starting socket connection...");
            var singleNode = options.ConnectionOptions.Nodes.Single();

            _socket = CreateSocket(options);

            await ConnectAsync(_socket, singleNode.Uri.Host, singleNode.Uri.Port);
            _stream = new NetworkStream(_socket, true);
            options.LogWriter?.WriteLine("Socket connection established.");
        }

        public async Task Write(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();

            await _stream.WriteAsync(buffer, offset, count);
        }

        public async Task Flush()
        {
            CheckConnectionStatus();

            await _stream.FlushAsync();
        }

        public async Task<int> Read(byte[] buffer, int offset, int count)
        {
            CheckConnectionStatus();

            var result = await _stream.ReadAsync(buffer, offset, count);

            if (result == 0)
            {
                Interlocked.Exchange(ref _stream, null)?.Dispose();
            }

            return result;
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

        private void CheckConnectionStatus()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(NetworkStreamPhysicalConnection));
            }

            if (_stream == null)
            {
                throw ExceptionHelper.NotConnected();
            }
        }

        private static Socket CreateSocket(ClientOptions options)
        {
            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
            {
                ReceiveTimeout = options.ConnectionOptions.ReceiveTimeout,
                SendTimeout = options.ConnectionOptions.SendTimeout,
                ReceiveBufferSize = options.ConnectionOptions.ReceiveBufferSize,
                SendBufferSize = options.ConnectionOptions.SendBufferSize
            };

            var size = sizeof(uint);
            uint on = 1;
            uint keepAliveInterval = 5000; //Send a packet once every 10 seconds.
            uint retryInterval = 500; //If no response, resend every second.
            var inArray = new byte[size * 3];
            Array.Copy(BitConverter.GetBytes(on), 0, inArray, 0, size);
            Array.Copy(BitConverter.GetBytes(keepAliveInterval), 0, inArray, size, size);
            Array.Copy(BitConverter.GetBytes(retryInterval), 0, inArray, size * 2, size);
            socket.IOControl(IOControlCode.KeepAliveValues, inArray, null);

            return socket;
        }
    }
}