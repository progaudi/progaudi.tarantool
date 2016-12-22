using System;
using System.Threading;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client
{
    public class LogicalConnectionManager : ILogicalConnection
    {
        private readonly ClientOptions _clientOptions;

        private readonly RequestIdCounter _requestIdCounter = new RequestIdCounter();

        private LogicalConnection _droppableLogicalConnection;

        private readonly ManualResetEvent _connected = new ManualResetEvent(false);

        private readonly AutoResetEvent _reconnectAvailable = new AutoResetEvent(true);

        private Timer _timer;

        private int _disposing;

        private const int _connectionTimeout = 1000;

        private const int _pingTimerInterval = 100;

        private const int _pingCheckInterval = 1000;

        private DateTimeOffset _nextPingTime = DateTimeOffset.MinValue;

        public LogicalConnectionManager(ClientOptions options)
        {
            _clientOptions = options;
        }

        public void Dispose()
        {
            if (Interlocked.Exchange(ref _disposing, 1) > 0)
            {
                return;
            }

            Interlocked.Exchange(ref _droppableLogicalConnection, null)?.Dispose();
            Interlocked.Exchange(ref _timer, null)?.Dispose();
        }

        public async Task Connect()
        {
            _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Connecting...");

            _connected.Reset();

            var _newConnection = new LogicalConnection(_clientOptions, _requestIdCounter);
            await _newConnection.Connect();
            Interlocked.Exchange(ref _droppableLogicalConnection, _newConnection)?.Dispose();

            _connected.Set();

            _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Connected...");

            _timer = new Timer(x => CheckPing(), null, _pingTimerInterval, Timeout.Infinite);
        }

        private static readonly PingRequest _pingRequest = new PingRequest();

        private void CheckPing()
        {
            try
            {
                LogicalConnection savedConnection = _droppableLogicalConnection;

                if (_nextPingTime > DateTimeOffset.UtcNow || savedConnection == null || !savedConnection.IsConnected())
                {
                    return;
                }

                Task task = savedConnection.SendRequestWithEmptyResponse(_pingRequest);
                if (Task.WaitAny(task) != 0 || task.Status != TaskStatus.RanToCompletion)
                {
                    _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Ping failed, dropping logical conection...");
                    savedConnection.Dispose();
                }
            }
            finally
            {
                if (_disposing == 0)
                {
                    _timer?.Change(_pingTimerInterval, Timeout.Infinite);
                }
            }
        }

        public bool IsConnected()
        {
            return _droppableLogicalConnection?.IsConnected() ?? false;
        }

        private async Task EnsureConnection()
        {
            if (_connected.WaitOne(_connectionTimeout) && IsConnected())
            {
                return;
            }

            _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Connection lost, wait for reconnect...");

            if (!_reconnectAvailable.WaitOne(_connectionTimeout))
            {
                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Failed to get lock for reconnect");
                throw ExceptionHelper.NotConnected();
            }

            try
            {
                if (!IsConnected())
                {
                    await Connect();
                }

                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Connection reacquired");
            }
            finally
            {
                _reconnectAvailable.Set();
            }
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            await EnsureConnection();
            var result = await _droppableLogicalConnection.SendRequest<TRequest, TResponse>(request);
            _nextPingTime = DateTimeOffset.UtcNow.AddMilliseconds(_pingCheckInterval);
            return result;
        }

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request) where TRequest : IRequest
        {
            await EnsureConnection();
            await _droppableLogicalConnection.SendRequestWithEmptyResponse(request);
            _nextPingTime = DateTimeOffset.UtcNow.AddMilliseconds(_pingCheckInterval);
        }
    }
}
