using System;
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
    public class LogicalConnection
    {
        private readonly MsgPackContext _msgPackContext;

        private readonly NetworkStreamPhysicalConnection _physicalConnection;

        private RequestId _currentRequestId = new RequestId(0);

        private readonly Dictionary<RequestId, TaskCompletionSource<MemoryStream>> _pendingRequests =
            new Dictionary<RequestId, TaskCompletionSource<MemoryStream>>();

        public LogicalConnection(ConnectionOptions options, NetworkStreamPhysicalConnection physicalConnection)
        {
            _msgPackContext = options.MsgPackContext;
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
            if (!_pendingRequests.TryGetValue(requestId, out request))
            {
                throw ExceptionHelper.WrongRequestId(requestId);
            }

            _pendingRequests.Remove(requestId);

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
            var serializedRequest = MsgPackSerializer.Serialize(request, _msgPackContext);

            var requestId = GetRequestId();
            var responseTask = GetResponseTask(requestId);

            long headerLength;
            var headerBuffer = CreateAndSerializeBuffer(request, requestId, serializedRequest, out headerLength);

            await _physicalConnection.Write(headerBuffer, 0, Constants.PacketSizeBufferSize + (int)headerLength);
            await _physicalConnection.Write(serializedRequest, 0, serializedRequest.Length);

            var responseBytes = await responseTask;

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
            var ulongRequestId = (long)_currentRequestId.Value;
            Interlocked.CompareExchange(ref ulongRequestId, 0, Constants.MaxRequestId);
            Interlocked.Increment(ref ulongRequestId);

            return _currentRequestId;
        }

        private Task<MemoryStream> GetResponseTask(RequestId requestId)
        {
            var tcs = new TaskCompletionSource<MemoryStream>();
            _pendingRequests.Add(requestId, tcs);
            return tcs.Task;
        }
    }
}