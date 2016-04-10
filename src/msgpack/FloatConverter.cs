using System.IO;
using System.Runtime.InteropServices;

namespace TarantoolDnx.MsgPack
{
    internal class FloatConverter : IMsgPackConverter<float>, IMsgPackConverter<double>
    {
        public void Write(float value, Stream stream, MsgPackSettings settings)
        {
            var binary = new FloatBinary(value);
            stream.WriteByte((byte) DataTypes.Single);
            stream.WriteByte(binary.byte0);
            stream.WriteByte(binary.byte1);
            stream.WriteByte(binary.byte2);
            stream.WriteByte(binary.byte3);
        }

        public void Write(double value, Stream stream, MsgPackSettings settings)
        {
            var binary = new DoubleBinary(value);
            stream.WriteByte((byte) DataTypes.Double);
            stream.WriteByte(binary.byte0);
            stream.WriteByte(binary.byte1);
            stream.WriteByte(binary.byte2);
            stream.WriteByte(binary.byte3);
            stream.WriteByte(binary.byte4);
            stream.WriteByte(binary.byte5);
            stream.WriteByte(binary.byte6);
            stream.WriteByte(binary.byte7);
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatBinary
        {
            [FieldOffset(0)] private readonly float value;

            [FieldOffset(0)] public readonly byte byte0;
            [FieldOffset(1)] public readonly byte byte1;
            [FieldOffset(2)] public readonly byte byte2;
            [FieldOffset(3)] public readonly byte byte3;

            public FloatBinary(float f)
            {
                this = default(FloatBinary);
                value = f;
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DoubleBinary
        {
            [FieldOffset(0)] private readonly double value;

            [FieldOffset(0)] public readonly byte byte0;
            [FieldOffset(1)] public readonly byte byte1;
            [FieldOffset(2)] public readonly byte byte2;
            [FieldOffset(3)] public readonly byte byte3;
            [FieldOffset(4)] public readonly byte byte4;
            [FieldOffset(5)] public readonly byte byte5;
            [FieldOffset(6)] public readonly byte byte6;
            [FieldOffset(7)] public readonly byte byte7;

            public DoubleBinary(double f)
            {
                this = default(DoubleBinary);
                value = f;
            }
        }
    }
}