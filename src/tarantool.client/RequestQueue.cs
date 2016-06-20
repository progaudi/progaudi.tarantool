using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    public class RequestQueue : IRequestQueue
    {
        private readonly Dictionary<ulong, TaskCompletionSource<byte[]>> _pendingRequests = new Dictionary<ulong, TaskCompletionSource<byte[]>>();

        public Task<byte[]> Queue(ulong requestId)
        {
            var tcs = new TaskCompletionSource<byte[]>();
            _pendingRequests.Add(requestId, tcs);

            return tcs.Task;
        }

        public void Dequeue(ulong requestId, byte[] result)
        {
            TaskCompletionSource<byte[]> request;
            if (!_pendingRequests.TryGetValue(requestId, out request))
            {
                throw new ArgumentOutOfRangeException($"Can't find pending request with id = {requestId}");
            }

            request.SetResult(result);
        }

    }
}