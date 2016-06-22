﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto;
using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;
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

        public async Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request) where TRequest : IRequestPacket
        {
            var serializedRequest = MsgPackSerializer.Serialize(request, _msgPackContext);

            var requestId = GetRequestId();
            var responseTask = GetResponseTask(requestId);

            long headerLength;
            var headerBuffer = CreateAndSerializeBuffer(request, requestId, serializedRequest, out headerLength);

            await _physicalConnection.WriteAsync(headerBuffer, 0, Constants.PacketSizeBufferSize + (int)headerLength);
            await _physicalConnection.WriteAsync(serializedRequest, 0, serializedRequest.Length);

            var responseBytes = await responseTask;

            var deserializedResponse = MsgPackSerializer.Deserialize<TResponse>(responseBytes, _msgPackContext);
            return deserializedResponse;
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

        private byte[] CreateAndSerializeBuffer<TRequest>(
            TRequest request,
            RequestId requestId,
            byte[] serializedRequest,
            out long headerLength) where TRequest : IRequestPacket
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