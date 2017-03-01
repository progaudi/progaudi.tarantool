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

        private readonly ManualResetEvent _connected = new ManualResetEvent(true);

        private readonly AutoResetEvent _reconnectAvailable = new AutoResetEvent(true);

        private Timer _timer;

        private int _disposing;

        private const int _connectionTimeout = 1000;

        private const int _pingTimerInterval = 100;

        private readonly int _pingCheckInterval = 1000;

        private DateTimeOffset _nextPingTime = DateTimeOffset.MinValue;

        public LogicalConnectionManager(ClientOptions options)
        {
            _clientOptions = options;

            if (_clientOptions.ConnectionOptions.PingCheckInterval >= 0)
            {
                _pingCheckInterval = _clientOptions.ConnectionOptions.PingCheckInterval;
            }
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
            if (IsConnectedInternal())
            {
                return;
            }

            if (!_reconnectAvailable.WaitOne(_connectionTimeout))
            {
                throw ExceptionHelper.NotConnected();
            }

            try
            {
                if (IsConnectedInternal())
                {
                    return;
                }

                _connected.Reset();

                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Connecting...");

                var _newConnection = new LogicalConnection(_clientOptions, _requestIdCounter);
                await _newConnection.Connect();
                Interlocked.Exchange(ref _droppableLogicalConnection, _newConnection)?.Dispose();

                _connected.Set();

                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Connected...");

                if (_pingCheckInterval > 0 && _timer == null)
                {
                    _timer = new Timer(x => CheckPing(), null, _pingTimerInterval, Timeout.Infinite);
                }
            }
            finally
            {
                _reconnectAvailable.Set();
            }
        }

        private static readonly PingRequest _pingRequest = new PingRequest();

        private void CheckPing()
        {
            try
            {
                if (_nextPingTime > DateTimeOffset.UtcNow)
                {
                    return;
                }

                SendRequestWithEmptyResponse(_pingRequest).GetAwaiter().GetResult();
            }
            catch (Exception e)
            {
                _clientOptions.LogWriter?.WriteLine($"{nameof(LogicalConnectionManager)}: Ping failed with exception: {e.Message}. Dropping current connection.");
                _droppableLogicalConnection?.Dispose();
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
            if (!_connected.WaitOne(_connectionTimeout))
            {
                return false;
            }

            return IsConnectedInternal();
        }

        private bool IsConnectedInternal()
        {
            return _droppableLogicalConnection?.IsConnected() ?? false;
        }

        private void ScheduleNextPing()
        {
            if (_pingCheckInterval > 0)
            {
                _nextPingTime = DateTimeOffset.UtcNow.AddMilliseconds(_pingCheckInterval);
            }
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequest
        {
            await Connect();

            var result = await _droppableLogicalConnection.SendRequest<TRequest, TResponse>(request);

            ScheduleNextPing();

            return result;
        }

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request) where TRequest : IRequest
        {
            await Connect();

            await _droppableLogicalConnection.SendRequestWithEmptyResponse(request);

            ScheduleNextPing();
        }
    }
}
