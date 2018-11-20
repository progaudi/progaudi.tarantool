using System;
using System.Buffers;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class LogicalConnection : ILogicalConnection
    {
        private readonly MsgPackContext _msgPackContext;

        private readonly ClientOptions _clientOptions;

        private readonly RequestIdCounter _requestIdCounter;

        private readonly IPhysicalConnection _physicalConnection;

        private readonly IResponseReader _responseReader;

        private readonly IRequestWriter _requestWriter;

        private readonly ILog _logWriter;

        private bool _disposed;
        private readonly IMsgPackFormatter<RequestHeader> _headerFormatter;

        public LogicalConnection(ClientOptions options, RequestIdCounter requestIdCounter)
        {
            _clientOptions = options;
            _requestIdCounter = requestIdCounter;
            _msgPackContext = options.MsgPackContext;
            _logWriter = options.LogWriter;

            _physicalConnection = new NetworkStreamPhysicalConnection();
            _responseReader = new ResponseReader(_clientOptions, _physicalConnection);
            _requestWriter = new RequestWriter(_clientOptions, _physicalConnection);
            _headerFormatter = _msgPackContext.GetFormatter<RequestHeader>();
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

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            await SendRequestImpl(request, timeout).ConfigureAwait(false);
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            var memory = await SendRequestImpl(request, timeout).ConfigureAwait(false);
            return MsgPackSerializer.Deserialize<DataResponse<TResponse[]>>(memory.Span, _msgPackContext);
        }

        public async Task<DataResponse> SendRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            var memory = await SendRequestImpl(request, timeout).ConfigureAwait(false);
            return MsgPackSerializer.Deserialize<DataResponse>(memory.Span, _msgPackContext);
        }

        public async Task<byte[]> SendRawRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
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

            var authenticateRequest = AuthenticationRequest.Create(greetings, singleNode);

            await SendRequestWithEmptyResponse(authenticateRequest).ConfigureAwait(false);
            _clientOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
        }

        private async Task<ReadOnlyMemory<byte>> SendRequestImpl<TRequest>(TRequest request, TimeSpan? timeout)
            where TRequest : IRequest
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LogicalConnection));
            }

            var formatter = _msgPackContext.GetRequiredFormatter<TRequest>();
            var expectedLength = Constants.PacketSizeBufferSize + Constants.MaxHeaderLength + formatter.GetBufferSize(request);
            using (var bodyBuffer = MemoryPool<byte>.Shared.Rent(expectedLength))
            {
                var requestId = _requestIdCounter.GetRequestId();
                var responseTask = _responseReader.GetResponseTask(requestId);
                var requestBuffer = bodyBuffer.Memory.Slice(Constants.PacketSizeBufferSize);

                var requestHeader = new RequestHeader(request.Code, requestId);
                var headerLength = _headerFormatter.Format(requestBuffer.Span, requestHeader);
                var bodyLength = formatter.Format(requestBuffer.Slice(headerLength).Span, request);
                var dataLength = headerLength + bodyLength;
                MsgPackSpec.WriteFixUInt32(bodyBuffer.Memory.Span, (uint) dataLength);

                var package = bodyBuffer.Memory.Slice(0, Constants.PacketSizeBufferSize + dataLength);
                _requestWriter.Write(package);

                try
                {
                    if (timeout.HasValue)
                    {
                        var cts = new CancellationTokenSource(timeout.Value);
                        responseTask = responseTask.WithCancellation(cts.Token);
                    }

                    var responseStream = await responseTask.ConfigureAwait(false);
                    _logWriter?.WriteLine($"Response with requestId {requestId} is received, length: {responseStream.Length}.");

                    return responseStream;
                }
                catch (ArgumentException)
                {
                    _logWriter?.WriteLine($"Response with requestId {requestId} failed, package:\n{ByteUtils.ToReadableString(package.Span)}");
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
}
