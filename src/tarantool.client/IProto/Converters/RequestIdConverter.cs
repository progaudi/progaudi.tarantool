using System;
using System.Linq;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.Utils;

namespace Tarantool.Client.IProto.Converters
{
    public class RequestIdConverter : IMsgPackConverter<RequestId>
    {
        public void Initialize(MsgPackContext context)
        {
                
        }

        public void Write(RequestId value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.UInt64);

            var requestIdBytes = BitConverter.GetBytes(value.Value).Reverse().ToArray();
            writer.Write(requestIdBytes);
        }

        public RequestId Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt64)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt64, type);
            }

            var allbytes = reader.ReadBytes(8);
            var requestIdBytes = allbytes.Array.Skip(allbytes.Offset).Take(allbytes.Count).Reverse().ToArray();

            return new RequestId(BitConverter.ToUInt64(requestIdBytes, 0));
        }
    }
}