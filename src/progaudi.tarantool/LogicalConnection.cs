using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MessagePack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class LogicalConnection : ILogicalConnection
    {
        private readonly ClientOptions _clientOptions;

        private readonly RequestIdCounter _requestIdCounter;

        private readonly IPhysicalConnection _physicalConnection;

        private readonly IResponseReader _responseReader;

        private readonly IRequestWriter _requestWriter;

        private readonly ILog _logWriter;

        private bool _disposed;

        public LogicalConnection(ClientOptions options, RequestIdCounter requestIdCounter)
        {
            _clientOptions = options;
            _requestIdCounter = requestIdCounter;
            _logWriter = options.LogWriter;

            _physicalConnection = new NetworkStreamPhysicalConnection();
            _responseReader = new ResponseReader(_clientOptions, _physicalConnection);
            _requestWriter = new RequestWriter(_clientOptions, _physicalConnection);
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

            _responseReader.Dispose();
            _requestWriter.Dispose();
            _physicalConnection.Dispose();
        }

        public async Task Connect()
        {
            await _physicalConnection.Connect(_clientOptions).ConfigureAwait(false);

            var greetingsResponseBytes = new byte[128];
            var readCount = await _physicalConnection.ReadAsync(greetingsResponseBytes, 0, greetingsResponseBytes.Length).ConfigureAwait(false);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            _clientOptions.LogWriter?.WriteLine($"Greetings received, salt is {Convert.ToBase64String(greetings.Salt)} .");

            PingsFailedByTimeoutCount = 0;

            _responseReader.BeginReading();
            _requestWriter.BeginWriting();

            _clientOptions.LogWriter?.WriteLine("Server responses reading started.");

            await LoginIfNotGuest(greetings).ConfigureAwait(false);
        }

        public bool IsConnected()
        {
            if (_disposed)
            {
                return false;
            }

            return _responseReader.IsConnected && _requestWriter.IsConnected && _physicalConnection.IsConnected;
        }

        public Task SendRequestWithEmptyResponse<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return SendRequestImpl(request, timeout);
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            var (result, bodyStart) = await SendRequestImpl(request, timeout).ConfigureAwait(false);
            var formatter = MessagePackSerializer.DefaultResolver.GetFormatter<DataResponse<TResponse[]>>();
            return formatter.Deserialize(result, bodyStart, MessagePackSerializer.DefaultResolver, out _);
        }

        public async Task<DataResponse> SendRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            var (result, bodyStart) = await SendRequestImpl(request, timeout).ConfigureAwait(false);
            var formatter = MessagePackSerializer.DefaultResolver.GetFormatter<DataResponse>();
            return formatter.Deserialize(result, bodyStart, MessagePackSerializer.DefaultResolver, out _);
        }

        public async Task<byte[]> SendRawRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return (await SendRequestImpl(request, timeout).ConfigureAwait(false)).result;
        }

        private async Task LoginIfNotGuest(GreetingsResponse greetings)
        {
            var singleNode = _clientOptions.ConnectionOptions.Nodes.Single();

            if (string.IsNullOrEmpty(singleNode.Uri.UserName))
            {
                _clientOptions.LogWriter?.WriteLine("Guest mode, no authentication attempt.");
                return;
            }

            var authenticateRequest = AuthenticationRequest.Create(greetings, singleNode.Uri);

            await SendRequestWithEmptyResponse(authenticateRequest).ConfigureAwait(false);
            _clientOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
        }

        private async Task<(byte[] result, int bodyStart)> SendRequestImpl<TRequest>(TRequest request, TimeSpan? timeout)
            where TRequest : IRequest
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LogicalConnection));
            }

            var bodyBuffer = MessagePackSerializer.Serialize(request);

            var requestId = _requestIdCounter.GetRequestId();
            var responseTask = _responseReader.GetResponseTask(requestId);

            var headerBuffer = MessagePackSerializer.Serialize(new RequestHeader(request.Code, requestId));
            _requestWriter.Write(headerBuffer, bodyBuffer);

            try
            {
                if (timeout.HasValue)
                {
                    var cts = new CancellationTokenSource(timeout.Value);
                    responseTask = responseTask.WithCancellation(cts.Token);
                }

                var (result, bodyStart) = await responseTask.ConfigureAwait(false);
                _logWriter?.WriteLine($"Response with requestId {requestId} is recieved, length: {result.Length}.");

                return (result, bodyStart);
            }
            catch (ArgumentException)
            {
                _logWriter?.WriteLine($"Response with requestId {requestId} failed, header:\n{headerBuffer.ToReadableString()} \n body: \n{bodyBuffer.ToReadableString()}");
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