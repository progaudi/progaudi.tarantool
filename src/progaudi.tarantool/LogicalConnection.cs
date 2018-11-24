using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class LogicalConnection : ILogicalConnection
    {
        private readonly MsgPackContext _msgPackContext;

        private readonly ClientOptions _clientOptions;

        private readonly IPhysicalConnection _physicalConnection;

        private readonly ILog _logWriter;

        private bool _disposed;

        public LogicalConnection(ClientOptions options)
        {
            _clientOptions = options;
            _msgPackContext = options.MsgPackContext;
            _logWriter = options.LogWriter;

            _physicalConnection = new NetworkStreamPhysicalConnection(_clientOptions);
        }

        public uint PingsFailedByTimeoutCount
        {
            get;
            private set;
        }

        public void Dispose()
        {
            if (_disposed)
            {
                return;
            }

            _disposed = true;
            _physicalConnection.Dispose();
        }

        public async Task Connect()
        {
            var greetingsResponseBytes = await _physicalConnection.Connect(_clientOptions).ConfigureAwait(false);
            if (greetingsResponseBytes.Length != Constants.GreetingsSize)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(greetingsResponseBytes.Length);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes.Span);

            _clientOptions.LogWriter?.WriteLine($"Greetings received, salt is {Convert.ToBase64String(greetings.Salt)} .");

            PingsFailedByTimeoutCount = 0;

            _clientOptions.LogWriter?.WriteLine("Server responses reading started.");

            await LoginIfNotGuest(greetings).ConfigureAwait(false);
        }

        public bool IsConnected()
        {
            return !_disposed && _physicalConnection.IsConnected;
        }

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : Request
        {
            await SendRequestImpl(request, timeout).ConfigureAwait(false);
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : Request
        {
            var memory = await SendRequestImpl(request, timeout).ConfigureAwait(false);
            return MsgPackSerializer.Deserialize<DataResponse<TResponse[]>>(memory.Span, _msgPackContext);
        }

        public async Task<DataResponse> SendRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : Request
        {
            var memory = await SendRequestImpl(request, timeout).ConfigureAwait(false);
            return MsgPackSerializer.Deserialize<DataResponse>(memory.Span, _msgPackContext);
        }

        public async Task<byte[]> SendRawRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : Request
        {
            return (await SendRequestImpl(request, timeout).ConfigureAwait(false)).ToArray();
        }

        private async Task LoginIfNotGuest(GreetingsResponse greetings)
        {
            if (! _clientOptions.ConnectionOptions.Nodes.Any()) 
                throw new ClientSetupException("There are zero configured nodes, you should provide one");

            var singleNode = _clientOptions.ConnectionOptions.Nodes.Single();

            if (string.IsNullOrEmpty(singleNode.User))
            {
                _clientOptions.LogWriter?.WriteLine("Guest mode, no authentication attempt.");
                return;
            }

            var authenticateRequest = AuthenticationRequest.Create(greetings, singleNode, _msgPackContext);

            await SendRequestWithEmptyResponse(authenticateRequest).ConfigureAwait(false);
            _clientOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
        }

        private async Task<ReadOnlyMemory<byte>> SendRequestImpl<TRequest>(TRequest request, TimeSpan? timeout)
            where TRequest : Request
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LogicalConnection));
            }

            var responseTask = _physicalConnection.Reader.GetResponseTask(request.Header.Id);
            _physicalConnection.Writer.Write(request);

            try
            {
                if (timeout.HasValue)
                {
                    var cts = new CancellationTokenSource(timeout.Value);
                    responseTask = responseTask.WithCancellation(cts.Token);
                }

                var responseStream = await responseTask.ConfigureAwait(false);
                _logWriter?.WriteLine($"Response with requestId {request.Header.Id} is received, length: {responseStream.Length}.");

                return responseStream;
            }
            catch (ArgumentException)
            {
                _logWriter?.WriteLine($"Response with requestId {request.Header.Id} failed");
                throw;
            }
            catch (TimeoutException)
            {
                PingsFailedByTimeoutCount++;
                throw;
            }
        }
    }
}
