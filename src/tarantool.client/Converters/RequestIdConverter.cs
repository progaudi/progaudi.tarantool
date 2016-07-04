using System;
using System.Runtime.InteropServices;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    internal class RequestIdConverter : IMsgPackConverter<RequestId>
    {
        public void Initialize(MsgPackContext context)
        {
                
        }

        public void Write(RequestId value, IMsgPackWriter writer)
        {
            writer.Write(DataTypes.UInt64);

            var binary = new ULongBinary(value.Value);

            byte[] bytes;
            if (BitConverter.IsLittleEndian)
            {
                bytes = new[]
                {
                    binary.byte7,
                    binary.byte6,
                    binary.byte5,
                    binary.byte4,
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
                    binary.byte4,
                    binary.byte5,
                    binary.byte6,
                    binary.byte7
                };
            }

            writer.Write(bytes);
        }

        public RequestId Read(IMsgPackReader reader)
        {
            var type = reader.ReadDataType();
            if (type != DataTypes.UInt64)
            {
                throw ExceptionHelper.UnexpectedDataType(DataTypes.UInt64, type);
            }

            var allbytes = reader.ReadBytes(8);
            var ulongValue = new ULongBinary(allbytes);
            return new RequestId(ulongValue.value);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct ULongBinary
        {
            [FieldOffset(0)]
            public readonly ulong value;

            [FieldOffset(0)]
            public readonly byte byte0;

            [FieldOffset(1)]
            public readonly byte byte1;

            [FieldOffset(2)]
            public readonly byte byte2;

            [FieldOffset(3)]
            public readonly byte byte3;

            [FieldOffset(4)]
            public readonly byte byte4;

            [FieldOffset(5)]
            public readonly byte byte5;

            [FieldOffset(6)]
            public readonly byte byte6;

            [FieldOffset(7)]
            public readonly byte byte7;

            public ULongBinary(ulong f)
            {
                this = default(ULongBinary);
                value = f;
            }

            public ULongBinary(ArraySegment<byte> bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes.Array[bytes.Offset + 7];
                    byte1 = bytes.Array[bytes.Offset + 6];
                    byte2 = bytes.Array[bytes.Offset + 5];
                    byte3 = bytes.Array[bytes.Offset + 4];
                    byte4 = bytes.Array[bytes.Offset + 3];
                    byte5 = bytes.Array[bytes.Offset + 2];
                    byte6 = bytes.Array[bytes.Offset + 1];
                    byte7 = bytes.Array[bytes.Offset + 0];
                }
                else
                {
                    byte0 = bytes.Array[bytes.Offset + 0];
                    byte1 = bytes.Array[bytes.Offset + 1];
                    byte2 = bytes.Array[bytes.Offset + 2];
                    byte3 = bytes.Array[bytes.Offset + 3];
                    byte4 = bytes.Array[bytes.Offset + 4];
                    byte5 = bytes.Array[bytes.Offset + 5];
                    byte6 = bytes.Array[bytes.Offset + 6];
                    byte7 = bytes.Array[bytes.Offset + 7];
                }
            }
        }
    }
}