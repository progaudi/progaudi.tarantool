using System;
using System.Linq;
using System.Runtime.InteropServices;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    internal class PacketSizeConverter : IMsgPackConverter<PacketSize>
    {
        public void Initialize(MsgPackContext context)
        {

        }

        public void Write(PacketSize value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.UInt32);

            var binary = new UIntBinary(value.Value);

            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
                    binary.byte3,
                    binary.byte2,
                    binary.byte1,
                    binary.byte0
                };
            }
            else
            {
                bytes = new[]
                {
                    binary.byte0,
                    binary.byte1,
                    binary.byte2,
                    binary.byte3,
                };
            }

            writer.Write(bytes);
        }

        public PacketSize Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt32)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt32, type);
            }

            var allbytes = reader.ReadBytes(4);
            var uintValue = new UIntBinary(allbytes);
            return new PacketSize(uintValue.value);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct UIntBinary
        {
            [FieldOffset(0)]
            public readonly uint value;

            [FieldOffset(0)]
            public readonly byte byte0;

            [FieldOffset(1)]
            public readonly byte byte1;

            [FieldOffset(2)]
            public readonly byte byte2;

            [FieldOffset(3)]
            public readonly byte byte3;

            public UIntBinary(uint f)
            {
                this = default(UIntBinary);
                value = f;
            }

            public UIntBinary(ArraySegment<byte> bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes.Array[bytes.Offset + 7];
                    byte1 = bytes.Array[bytes.Offset + 6];
                    byte2 = bytes.Array[bytes.Offset + 5];
                    byte3 = bytes.Array[bytes.Offset + 4];
                }
                else
                {
                    byte0 = bytes.Array[bytes.Offset + 0];
                    byte1 = bytes.Array[bytes.Offset + 1];
                    byte2 = bytes.Array[bytes.Offset + 2];
                    byte3 = bytes.Array[bytes.Offset + 3];
                }
            }
        }
    }
}