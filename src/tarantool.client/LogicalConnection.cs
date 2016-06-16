using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public class LogicalConnection : ILogicalConnection
    {
        private const long MaxRequestId = long.MaxValue / 2;

        private readonly MsgPackContext _msgPackContext;

        private readonly IPhysicalConnection _physicalConnection;

        private const int MaxHeaderLength = 13;

        private const int LengthLength = 5;

        private RequestId _currentRequestId = new RequestId(0);

        private readonly Dictionary<ulong, TaskCompletionSource<byte[]>> _pendingRequests = new Dictionary<ulong, TaskCompletionSource<byte[]>>();

        public LogicalConnection(ConnectionOptions options)
        {
            _msgPackContext = options.MsgPackContext;
            _physicalConnection = options.PhysicalConnection;
        }

        public async Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequestPacket
        {
            var serializedRequest = MsgPackSerializer.Serialize(request, _msgPackContext);

            var buffer = new byte[LengthLength + MaxHeaderLength];
            var stream = new MemoryStream(buffer);

            var requestId = GetRequestId();
            var responseTask = GetResponseTask(requestId);

            var requestHeader = new RequestHeader(request.Code, requestId);
            stream.Seek(LengthLength, SeekOrigin.Begin);
            MsgPackSerializer.Serialize(requestHeader, stream, _msgPackContext);

            var headerLength = stream.Position - LengthLength;
            var packetLength = new PacketSize((uint)(headerLength + serializedRequest.Length));
            stream.Seek(0, SeekOrigin.Begin);
            MsgPackSerializer.Serialize(packetLength, stream, _msgPackContext);

            await _physicalConnection.WriteAsync(buffer, 0, LengthLength + (int)headerLength);
            await _physicalConnection.WriteAsync(serializedRequest, 0, serializedRequest.Length);

            var responseBytes = await responseTask;
            var deserializedResponse = MsgPackSerializer.Deserialize<TResponse>(responseBytes, _msgPackContext);
            return deserializedResponse;
        }

        public TaskCompletionSource<byte[]> GetResponseCompletionSource(ulong requestId)
        {
            TaskCompletionSource<byte[]> request;
            if (!_pendingRequests.TryGetValue(requestId, out request))
            {
                throw new ArgumentOutOfRangeException($"Can't find pending request with id = {requestId}");
            }

            return request;
        }

        private RequestId GetRequestId()
        {
            var ulongRequestId = (long)_currentRequestId.Value;
            Interlocked.CompareExchange(ref ulongRequestId, 0, MaxRequestId);
            Interlocked.Increment(ref ulongRequestId);

            return _currentRequestId;
        }

        private Task<byte[]> GetResponseTask(ulong requestId)
        {
            var tcs = new TaskCompletionSource<byte[]>();
            _pendingRequests.Add(requestId, tcs);
            return tcs.Task;
        }
    }
}