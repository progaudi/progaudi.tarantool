using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public class RequestWriter : IRequestWriter
    {
        private const long MaxRequestId = long.MaxValue / 2;

        private readonly MsgPackContext _msgPackContext;

        private readonly IPhysicalConnection _physicalConnection;

        private const int MaxHeaderLength = 12;

        private const int LengthLength = 5;

        private readonly Dictionary<ulong, TaskCompletionSource<byte[]>> _pendingRequests = new Dictionary<ulong, TaskCompletionSource<byte[]>>();

        private long _currentRequestId;

        public RequestWriter(MsgPackContext msgPackContext, IPhysicalConnection physicalConnection)
        {
            _msgPackContext = msgPackContext;
            _physicalConnection = physicalConnection;
        }

        public async Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequestPacket
        {
            var serializedRequest = MsgPackSerializer.Serialize(request, _msgPackContext);

            var packetLength = LengthLength + MaxHeaderLength + serializedRequest.Length;

            var buffer = new byte[LengthLength + MaxHeaderLength];
            var stream = new MemoryStream(buffer);
            MsgPackSerializer.Serialize(packetLength, stream, _msgPackContext);

            var requestId = GetRequestId();
            var requestHeader = new RequestHeader(request.Code, requestId);
            MsgPackSerializer.Serialize(requestHeader, stream, _msgPackContext);

            Write(buffer, 0, (int) stream.Position);
            Write(serializedRequest, 0, serializedRequest.Length);

            var tcs = new TaskCompletionSource<byte[]>();

            _pendingRequests.Add(requestId, tcs);

            var responseBytes = await tcs.Task;
            var deserializedResponse = MsgPackSerializer.Deserialize<TResponse>(responseBytes, _msgPackContext);
            return deserializedResponse;
        }

        public void CompleteRequest(ulong requestId, byte[] responseBytes)
        {
            TaskCompletionSource<byte[]> request;
            if (!_pendingRequests.TryGetValue(requestId, out request))
            {
                throw new ArgumentOutOfRangeException($"Can't find pending request with id = {requestId}");
            }

            request.SetResult(responseBytes);
        }

        private void Write(byte[] buffer, int offset, int count)
        {
            _physicalConnection.Write(buffer, offset, count);
        }

        private ulong GetRequestId()
        {
            Interlocked.CompareExchange(ref _currentRequestId, 0, MaxRequestId);
            Interlocked.Increment(ref _currentRequestId);

            return (ulong)_currentRequestId;
        }
    }
}