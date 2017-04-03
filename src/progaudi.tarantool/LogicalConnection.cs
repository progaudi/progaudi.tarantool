using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using ProGaudi.MsgPack.Light;

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

        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        private readonly ReaderWriterLockSlim _physicalConnectionLock = new ReaderWriterLockSlim();

        private readonly IResponseReader _responseReader;

        private readonly ILog _logWriter;

        private bool _disposed;

        public LogicalConnection(ClientOptions options, RequestIdCounter requestIdCounter)
        {
            _clientOptions = options;
            _requestIdCounter = requestIdCounter;
            _msgPackContext = options.MsgPackContext;
            _logWriter = options.LogWriter;

            _physicalConnection = new NetworkStreamPhysicalConnection();
            _responseReader = new ResponseReader(_clientOptions, _physicalConnection);
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
            _physicalConnection.Dispose();
        }

        public async Task Connect()
        {
            await _physicalConnection.Connect(_clientOptions);

            var greetingsResponseBytes = new byte[128];
            var readCount = await _physicalConnection.ReadAsync(greetingsResponseBytes, 0, greetingsResponseBytes.Length);
            if (readCount != greetingsResponseBytes.Length)
            {
                throw ExceptionHelper.UnexpectedGreetingBytesCount(readCount);
            }

            var greetings = new GreetingsResponse(greetingsResponseBytes);

            _clientOptions.LogWriter?.WriteLine($"Greetings received, salt is {Convert.ToBase64String(greetings.Salt)} .");

            PingsFailedByTimeoutCount = 0;

            _responseReader.BeginReading();

            _clientOptions.LogWriter?.WriteLine("Server responses reading started.");

            await LoginIfNotGuest(greetings);
        }

        public bool IsConnected()
        {
            if (_disposed)
            {
                return false;
            }

            if (!_responseReader.IsConnected() || !_physicalConnection.IsConnected())
            {
                return false;
            }

            return true;
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

            await SendRequestWithEmptyResponse(authenticateRequest);
            _clientOptions.LogWriter?.WriteLine($"Authentication request send: {authenticateRequest}");
        }

        public Task SendRequestWithEmptyResponse<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return SendRequestImpl<TRequest, EmptyResponse>(request, timeout);
        }

        public Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            return SendRequestImpl<TRequest, DataResponse<TResponse[]>>(request, timeout);
        }

        public static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            var buffer = new byte[16*1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }

        private async Task<TResponse> SendRequestImpl<TRequest, TResponse>(TRequest request, TimeSpan? timeout)
            where TRequest : IRequest
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(nameof(LogicalConnection));
            }

            var bodyBuffer = MsgPackSerializer.Serialize(request, _msgPackContext);

            var requestId = _requestIdCounter.GetRequestId();
            var responseTask = _responseReader.GetResponseTask(requestId);

            long headerLength;
            var headerBuffer = CreateAndSerializeBuffer(request, requestId, bodyBuffer, out headerLength);

            try
            {
                _physicalConnectionLock.EnterWriteLock();

                _logWriter?.WriteLine($"Begin sending request header buffer, requestId: {requestId}, code: {request.Code}, length: {headerBuffer.Length}");
                _physicalConnection.Write(headerBuffer, 0, Constants.PacketSizeBufferSize + (int)headerLength);

                _logWriter?.WriteLine($"Begin sending request body buffer, length: {bodyBuffer.Length}");
                _physicalConnection.Write(bodyBuffer, 0, bodyBuffer.Length);
            }
            catch (Exception ex)
            {
                _logWriter?.WriteLine(
                    $"Request with requestId {requestId} failed, header:\n{ToReadableString(headerBuffer)} \n body: \n{ToReadableString(bodyBuffer)}");
                Dispose();
                throw;
            }
            finally
            {
                _physicalConnectionLock.ExitWriteLock();
            }

            try
            {
                if (timeout.HasValue)
                {
                    var cts = new CancellationTokenSource(timeout.Value);
                    responseTask = responseTask.WithCancellation(cts.Token);
                }

                var responseStream = await responseTask;
                _logWriter?.WriteLine($"Response with requestId {requestId} is recieved, length: {responseStream.Length}.");

                return MsgPackSerializer.Deserialize<TResponse>(responseStream, _msgPackContext);
            }
            catch (ArgumentException)
            {
                _logWriter?.WriteLine($"Response with requestId {requestId} failed, header:\n{ToReadableString(headerBuffer)} \n body: \n{ToReadableString(bodyBuffer)}");
                throw;
            }
            catch (TimeoutException)
            {
                PingsFailedByTimeoutCount++;
                throw;
            }
        }

        private static string ToReadableString(byte[] bytes)
        {
            return string.Join(" ", bytes.Select(b => b.ToString("X2")));
        }

        private byte[] CreateAndSerializeBuffer<TRequest>(
            TRequest request,
            RequestId requestId,
            byte[] serializedRequest,
            out long headerLength) where TRequest : IRequest
        {
            var packetSizeBuffer = new byte[Constants.PacketSizeBufferSize + Constants.MaxHeaderLength];
            var stream = new MemoryStream(packetSizeBuffer);

            var requestHeader = new RequestHeader(request.Code, requestId);
            stream.Seek(Constants.PacketSizeBufferSize, SeekOrigin.Begin);
            MsgPackSerializer.Serialize(requestHeader, stream, _msgPackContext);

            headerLength = stream.Position - Constants.PacketSizeBufferSize;
            var packetLength = new PacketSize((uint) (headerLength + serializedRequest.Length));
            stream.Seek(0, SeekOrigin.Begin);
            MsgPackSerializer.Serialize(packetLength, stream, _msgPackContext);
            return packetSizeBuffer;
        }
    }
}