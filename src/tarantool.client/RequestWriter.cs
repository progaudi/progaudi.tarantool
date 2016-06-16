using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    public class RequestWriter : IRequestWriter
    {
        private readonly IPhysicalConnection _physicalConnection;

        private readonly ConnectionOptions _connectionOptions;

        private readonly ConcurrentDictionary<ulong, TaskCompletionSource<byte[]>> _pendingRequests = new ConcurrentDictionary<ulong, TaskCompletionSource<byte[]>>();

        public RequestWriter(IPhysicalConnection stream, ConnectionOptions connectionOptions)
        {
            _physicalConnection = stream;
            _connectionOptions = connectionOptions;
        }

        public void EndRequest(ulong requestId, byte[] result)
        {
            TaskCompletionSource<byte[]> pendingRequest;
            if (_pendingRequests.TryGetValue(requestId, out pendingRequest))
            {
                _connectionOptions.LogWriter?.WriteLine(
                    pendingRequest.TrySetResult(result)
                        ? $"Successfully completed request with id:{requestId}"
                        : $"Can't complete request with id:{requestId}");
            }
            else
            {
                _connectionOptions.LogWriter?.WriteLine($"Can't find matching request for response with id: {requestId}.");
            }
        }

        public async Task<byte[]> WriteRequest(byte[] request, ulong requestId)
        {
            var requestTask = new TaskCompletionSource<byte[]>();

            if (_pendingRequests.TryAdd(requestId, requestTask))
            {
                await _physicalConnection.WriteAsync(request, 0, request.Length);
            }
            else
            {
                _connectionOptions.LogWriter?.WriteLine($"Request with such id ({requestId}) is already sent!");
                requestTask.SetResult(null);
            }

            return await requestTask.Task;
        }
    }
}