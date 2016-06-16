using System;
using System.Linq;

using MsgPack.Light;

using Shouldly;

namespace Tarantool.Client.IProto.Converters
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
            type.ShouldBe(DataTypes.UInt32);

            var requestIdBytes = reader.ReadBytes(4);
            return new PacketSize(BitConverter.ToUInt32(requestIdBytes.Array.Reverse().ToArray(), 0));
        }
    }
}