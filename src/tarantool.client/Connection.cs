using System;
using System.IO;
using System.Threading.Tasks;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public class Connection : IConnection
    {
        MsgPackContext _msgPackContext;

        private const int headerLength = 10;

        private const int lengthLength = 5;

        public async Task<TResponse> SendPacket<TRequest, TResponse>(TRequest request) where TRequest : IRequestPacket
        {
            var serializedRequest = MsgPackSerializer.Serialize(request, _msgPackContext);
            var packetLength = lengthLength + headerLength + serializedRequest.Length;
            var buffer = new byte[lengthLength + headerLength];
            var stream = new MemoryStream(buffer);
            MsgPackSerializer.Serialize(packetLength, stream, _msgPackContext);

            var requestHeader = new RequestHeader(request.Code, GetRequestId());
            MsgPackSerializer.Serialize(requestHeader, stream, _msgPackContext);

            await WriteAsync(buffer);
            await WriteAsync(serializedRequest);
            throw new NotImplementedException();
        }

        private async Task WriteAsync(byte[] buffer)
        {
            throw new NotImplementedException();
        }

        private ulong GetRequestId()
        {
            throw new NotImplementedException();
        }
    }
}