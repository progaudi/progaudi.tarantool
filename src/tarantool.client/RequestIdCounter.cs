namespace ProGaudi.Tarantool.Client
{
    using System.Threading;

    using ProGaudi.Tarantool.Client.Model;

    public class RequestIdCounter
    {
        private long _currentRequestId;

        public RequestId GetRequestId()
        {
            var requestId = Interlocked.Increment(ref _currentRequestId);
            return (RequestId)(ulong)requestId;
        }
    }
}
