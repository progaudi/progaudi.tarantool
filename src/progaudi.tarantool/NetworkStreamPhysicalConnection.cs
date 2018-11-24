using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Utils;

#if PROGAUDI_NETCORE
using System.Net;
#endif

namespace ProGaudi.Tarantool.Client
{
    internal class NetworkStreamPhysicalConnection : IPhysicalConnection
    {
        private readonly ClientOptions _clientOptions;
        
        private Stream _stream;

        private Socket _socket;

        private bool _disposed;
        private IResponseReader _reader;
        private IRequestWriter _writer;

        public NetworkStreamPhysicalConnection(ClientOptions clientOptions)
        {
            _clientOptions = clientOptions;
        }

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

        public async Task<ReadOnlyMemory<byte>> Connect(ClientOptions options)
        {
            if (! options.ConnectionOptions.Nodes.Any()) 
                throw new ClientSetupException("There are zero configured nodes, you should provide one");

            options.LogWriter?.WriteLine("Starting socket connection...");
            var singleNode = options.ConnectionOptions.Nodes.Single();

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp)
            {
                NoDelay = true
            };
            await ConnectAsync(_socket, singleNode.Host, singleNode.Port).ConfigureAwait(false);;

            _stream = new NetworkStream(_socket, true);
            options.LogWriter?.WriteLine("Socket connection established.");
            Memory<byte> result = new byte[Constants.GreetingsSize];
            var read = await _stream.ReadAsync(result);
                
            Writer = new SocketRequestWriter(_clientOptions, _stream);
            Writer.Start();
            Reader = new SocketResponseReader(_clientOptions, _stream);
            Reader.Start();

            return result.Slice(0, read);
        }

#if PROGAUDI_NETCORE
        /// https://github.com/mongodb/mongo-csharp-driver/commit/9c2097f349d5096a04ea81b0c9ceb60c7e1acee4
        private static async Task ConnectAsync(Socket socket, string host, int port)
        {
            var resolved = await Dns.GetHostAddressesAsync(host).ConfigureAwait(false);;
            for (var i = 0; i < resolved.Length; i++)
            {
                try
                {
                    await socket.ConnectAsync(resolved[i], port).ConfigureAwait(false);
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

        public bool IsConnected => !_disposed && _stream != null;

        public IResponseReader Reader
        {
            get
            {
                CheckConnectionStatus();
                return _reader;
            }
            private set => _reader = value;
        }

        public IRequestWriter Writer
        {
            get
            {
                CheckConnectionStatus();
                return _writer;
            }
            private set => _writer = value;
        }

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
