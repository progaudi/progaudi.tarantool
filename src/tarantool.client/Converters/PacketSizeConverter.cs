using System;
using System.Linq;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    public class PacketSizeConverter : IMsgPackConverter<PacketSize>
    {
        public void Initialize(MsgPackContext context)
        {

        }

        public void Write(PacketSize value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.UInt32);

            var requestIdBytes = BitConverter.GetBytes(value.Value).Reverse().ToArray();
            writer.Write(requestIdBytes);
        }

        public PacketSize Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt32)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt32, type);
            }

            var requestIdBytes = reader.ReadBytes(4);
            return new PacketSize(BitConverter.ToUInt32(requestIdBytes.Array.Reverse().ToArray(), 0));
        }
    }
}