using System;
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

        private long _currentRequestId = 0;

        private readonly ConcurrentDictionary<RequestId, TaskCompletionSource<MemoryStream>> _pendingRequests =
            new ConcurrentDictionary<RequestId, TaskCompletionSource<MemoryStream>>();

        private readonly ILog _logWriter;

        public LogicalConnection(ConnectionOptions options, INetworkStreamPhysicalConnection physicalConnection)
        {
            _msgPackContext = options.MsgPackContext;
            _logWriter = options.LogWriter;
            _physicalConnection = physicalConnection;
        }

        public async Task SendRequestWithEmptyResponse<TRequest>(TRequest request)
            where TRequest : IRequest
        {
            await SendRequestImpl<TRequest, EmptyResponse>(request);
        }

        public async Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest
        {
            return await SendRequestImpl<TRequest, DataResponse<TResponse[]>>(request);
        }

        public TaskCompletionSource<MemoryStream> PopResponseCompletionSource(RequestId requestId, MemoryStream resultStream)
        {
            TaskCompletionSource<MemoryStream> request;

            if (!_pendingRequests.TryRemove(requestId, out request))
            {
                throw ExceptionHelper.WrongRequestId(requestId);
            }

            return request;
        }

        public static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
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
                _physicalConnection.Write(headerBuffer, 0, Constants.PacketSizeBufferSize + (int)headerLength);

                _logWriter?.WriteLine($"Begin sending request body buffer, length: {bodyBuffer.Length}");
                _physicalConnection.Write(bodyBuffer, 0, bodyBuffer.Length);
            }

            try
            {
                var responseStream = await responseTask;
                _logWriter?.WriteLine($"Response with requestId {requestId} is recieved, length: {responseStream.Length}.");

                var deserializedResponse = MsgPackSerializer.Deserialize<TResponse>(responseStream, _msgPackContext);
                return deserializedResponse;
            }
            catch (ArgumentException e)
            {
                _logWriter?.WriteLine(
                    $"Response with requestId {requestId} failed, header:\n{ToReadableString(headerBuffer)} \n body: \n{ToReadableString(bodyBuffer)}");
                throw e;
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
            var packetLength = new PacketSize((uint)(headerLength + serializedRequest.Length));
            stream.Seek(0, SeekOrigin.Begin);
            MsgPackSerializer.Serialize(packetLength, stream, _msgPackContext);
            return packetSizeBuffer;
        }

        private RequestId GetRequestId()
        {
            var requestId = Interlocked.Increment(ref _currentRequestId);
            return (RequestId)(ulong)requestId;
        }

        private Task<MemoryStream> GetResponseTask(RequestId requestId)
        {
            var tcs = new TaskCompletionSource<MemoryStream>();
            if (!_pendingRequests.TryAdd(requestId, tcs))
            {
                throw ExceptionHelper.RequestWithSuchIdAlreadySent(requestId);
            }

            return tcs.Task;
        }
    }
}