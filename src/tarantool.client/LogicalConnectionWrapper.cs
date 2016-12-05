using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;

namespace ProGaudi.Tarantool.Client
{
    using System.Threading;

    using ProGaudi.Tarantool.Client.Model;
    using ProGaudi.Tarantool.Client.Utils;

    public class LogicalConnectionWrapper : ILogicalConnection
    {
        private readonly ClientOptions _clientOptions;

        private readonly RequestIdCounter _requestIdCounter = new RequestIdCounter();

        private LogicalConnection _droppableLogicalConnection;

        private readonly ManualResetEvent _connected = new ManualResetEvent(false);

        private const int connectionTimeout = 1000;

        public LogicalConnectionWrapper(ClientOptions options)
        {
            _clientOptions = options;
        }

        public void Dispose()
        {
            Interlocked.Exchange(ref _droppableLogicalConnection, null)?.Dispose();
        }

        public async Task Connect()
        {
            _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionWrapper)}: Connecting...");

            _connected.Reset();

            var _newConnection = new LogicalConnection(_clientOptions, _requestIdCounter);
            await _newConnection.Connect();
            Interlocked.Exchange(ref _droppableLogicalConnection, _newConnection)?.Dispose();

            _connected.Set();

            _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionWrapper)}: Connected...");
        }

        public bool IsConnected()
        {
            return _droppableLogicalConnection?.IsConnected() != null;
        }

        private async Task EnsureConnection()
        {
            if (_connected.WaitOne() && IsConnected())
            {
                return;
            }

            _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionWrapper)}: Connection lost, wait for reconnect...");

            if (!Monitor.TryEnter(this, connectionTimeout))
            {
                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionWrapper)}: Failed to get lock for reconnect");
                throw ExceptionHelper.NotConnected();
            }

            try
            {
                if (!IsConnected())
                {
                    await Connect();
                }

                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionWrapper)}: Connection reacquired");
            }
            finally
            {
                Monitor.Exit(this);
            }
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            await EnsureConnection();
            return await _droppableLogicalConnection.SendRequest<TRequest, TResponse>(request);
        }

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request) where TRequest : IRequest
        {
            await EnsureConnection();
            await _droppableLogicalConnection.SendRequestWithEmptyResponse(request);
        }
    }
}
