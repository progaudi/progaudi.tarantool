using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Headers;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Utils;

namespace Tarantool.Client
{
    internal class LogicalConnection : ILogicalConnection
    {
        private readonly MsgPackContext _msgPackContext;

        private readonly INetworkStreamPhysicalConnection _physicalConnection;

        private RequestId _currentRequestId = new RequestId(0);

        private readonly ConcurrentDictionary<RequestId, TaskCompletionSource<MemoryStream>> _pendingRequests =
            new ConcurrentDictionary<RequestId, TaskCompletionSource<MemoryStream>>();

        private readonly TextWriter _logWriter;

        public LogicalConnection(ConnectionOptions options, INetworkStreamPhysicalConnection physicalConnection)
        {
            _msgPackContext = options.MsgPackContext;
            _logWriter = options.LogWriter;
            _physicalConnection = physicalConnection;
        }

        public async Task SendRequestWithoutResponse<TRequest>(TRequest request)
            where TRequest : IRequest
        {
            await SendRequestImpl<TRequest, EmptyResponse>(request);
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest
        {
            return await SendRequestImpl<TRequest, DataResponse<TResponse[]>>(request);
        }

        public TaskCompletionSource<MemoryStream> PopResponseCompletionSource(RequestId requestId)
        {
            TaskCompletionSource<MemoryStream> request;

            if (!_pendingRequests.TryRemove(requestId, out request))
            {
                throw ExceptionHelper.WrongRequestId(requestId);
            }

            return request;
        }

        public IEnumerable<TaskCompletionSource<MemoryStream>> PopAllResponseCompletionSources()
        {
            var result = _pendingRequests.Values.ToArray();
            _pendingRequests.Clear();
            return result;
        }

        private async Task<TResponse> SendRequestImpl<TRequest, TResponse>(TRequest request)
         where TRequest : IRequest
        {
            var bodyBuffer = MsgPackSerializer.Serialize(request, _msgPackContext);

            var requestId = GetRequestId();
            var responseTask = GetResponseTask(requestId);

            long headerLength;
            var headerBuffer = CreateAndSerializeBuffer(request, requestId, bodyBuffer, out headerLength);

            lock (_physicalConnection)
            {
                _logWriter?.WriteLine($"Begin sending request header buffer, requestId: {requestId}, code: {request.Code}, length: {headerBuffer.Length}");
                _physicalConnection.Write(headerBuffer, 0, Constants.PacketSizeBufferSize + (int) headerLength);

                _logWriter?.WriteLine($"Begin sending request body buffer, length: {bodyBuffer.Length}");
                _physicalConnection.Write(bodyBuffer, 0, bodyBuffer.Length);
            }

            var responseBytes = await responseTask;
            _logWriter?.WriteLine($"Response with requestId {requestId} is recieved, length: {responseBytes.Length}.");

            var deserializedResponse = MsgPackSerializer.Deserialize<TResponse>(responseBytes, _msgPackContext);
            return deserializedResponse;
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
            var packetLength = new PacketSize((uint)(headerLength + serializedRequest.Length));
            stream.Seek(0, SeekOrigin.Begin);
            MsgPackSerializer.Serialize(packetLength, stream, _msgPackContext);
            return packetSizeBuffer;
        }

        private RequestId GetRequestId()
        {
            var longRequestId = (long)_currentRequestId.Value;
            Interlocked.Increment(ref longRequestId);
            _currentRequestId = (RequestId)(ulong)longRequestId;
            return _currentRequestId;
        }

        private Task<MemoryStream> GetResponseTask(RequestId requestId)
        {
            var tcs = new TaskCompletionSource<MemoryStream>();
            if (!_pendingRequests.TryAdd(requestId, tcs))
            {
                
            }
            return tcs.Task;
        }
    }
}