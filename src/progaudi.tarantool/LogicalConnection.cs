using System;
using System.Buffers;
using System.Buffers.Binary;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    internal class LogicalConnection : ILogicalConnection
    {
        private static readonly Func<MemoryStream, IMemoryOwner<byte>> RawByteCreator = x =>
        {
            var length = (int) (x.Length - x.Position);
            var owner = MemoryPool<byte>.Shared.Rent(length);
            new ReadOnlyMemory<byte>(x.GetBuffer(), (int) x.Position, length).CopyTo(owner.Memory);
            return owner;
        };

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
            return SendRequestImpl<TRequest, object>(request, _ => null, timeout);
        }

        public Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return SendRequestImpl(request, buffer => MsgPackSerializer.Deserialize<DataResponse<TResponse[]>>(buffer, _clientOptions.MsgPackContext), timeout);
        }

        public Task<DataResponse> SendRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return SendRequestImpl(request, buffer => MsgPackSerializer.Deserialize<DataResponse>(buffer, _clientOptions.MsgPackContext), timeout);
        }

        public Task<IMemoryOwner<byte>> SendRawRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return SendRequestImpl(request, RawByteCreator, timeout);
        }

        private async Task LoginIfNotGuest(GreetingsResponse greetings)
        {
            if (! _clientOptions.ConnectionOptions.Nodes.Any()) 
                throw new ClientSetupException("There are zero configured nodes, you should provide one");

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

        private Task<TResponse> SendRequestImpl<TRequest, TResponse>(TRequest request, Func<MemoryStream, TResponse> response, TimeSpan? timeout)
            where TRequest : IRequest
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LogicalConnection));
            }

            var requestId = _requestIdCounter.GetRequestId();
            var header = MsgPackSerializer.Serialize(new RequestHeader(request.Code, requestId), _clientOptions.MsgPackContext);
            var body = MsgPackSerializer.Serialize(request, _clientOptions.MsgPackContext);
            var length = new byte[5];
            length[0] = (byte) DataTypes.UInt32;
            BinaryPrimitives.WriteUInt32BigEndian(new Span<byte>(length, 1, 4), (uint) (header.Length + body.Length));
            var responseTask = _responseReader.GetResponseTask(requestId, response);
            var bodyBuffer = TarantoolSegment.CreateSequence(length, header, body);
            _requestWriter.Write(bodyBuffer);

            try
            {
                if (timeout.HasValue)
                {
                    var cts = new CancellationTokenSource(timeout.Value);
                    responseTask = responseTask.WithCancellation(cts.Token);
                }

                return responseTask;
            }
            catch (ArgumentException)
            {
                _logWriter?.WriteLine($"Request with requestId {requestId} failed, body: \n{bodyBuffer.ToReadableString()}");
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
