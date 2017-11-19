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

        private readonly TimeSpan? _pingTimeout;

        private DateTimeOffset _nextPingTime = DateTimeOffset.MinValue;

        public LogicalConnectionManager(ClientOptions options)
        {
            _clientOptions = options;

            if (_clientOptions.ConnectionOptions.PingCheckInterval >= 0)
            {
                _pingCheckInterval = _clientOptions.ConnectionOptions.PingCheckInterval;
            }

            _pingTimeout = _clientOptions.ConnectionOptions.PingCheckTimeout;
        }

        public uint PingsFailedByTimeoutCount => _droppableLogicalConnection?.PingsFailedByTimeoutCount ?? 0;

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

                var newConnection = new LogicalConnection(_clientOptions, _requestIdCounter);
                await newConnection.Connect().ConfigureAwait(false);;
                Interlocked.Exchange(ref _droppableLogicalConnection, newConnection)?.Dispose();

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

                SendRequestWithEmptyResponse(_pingRequest, _pingTimeout).GetAwaiter().GetResult();
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

        public bool IsConnected() => _connected.WaitOne(_connectionTimeout) && IsConnectedInternal();

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

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            await Connect().ConfigureAwait(false);

            var result = await _droppableLogicalConnection.SendRequest<TRequest, TResponse>(request, timeout).ConfigureAwait(false);

            ScheduleNextPing();

            return result;
        }

        public async Task<byte[]> SendRawRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest
        {
            await Connect().ConfigureAwait(false);

            var result = await _droppableLogicalConnection.SendRawRequest<TRequest>(request, timeout).ConfigureAwait(false);

            ScheduleNextPing();

            return result;
        }

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request, TimeSpan? timeout = null) where TRequest : IRequest
        {
            await Connect().ConfigureAwait(false);

            await _droppableLogicalConnection.SendRequestWithEmptyResponse(request, timeout).ConfigureAwait(false);

            ScheduleNextPing();
        }
    }
}
