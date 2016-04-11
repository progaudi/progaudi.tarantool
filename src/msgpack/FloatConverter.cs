using System;
using System.Runtime.InteropServices;

namespace TarantoolDnx.MsgPack
{
    internal class FloatConverter : IMsgPackConverter<float>, IMsgPackConverter<double>
    {
        public void Write(double value, IMsgPackWriter writer, MsgPackContext context)
        {
            var binary = new DoubleBinary(value);
            writer.Write(DataTypes.Double);
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(binary.byte7);
                writer.Write(binary.byte6);
                writer.Write(binary.byte5);
                writer.Write(binary.byte4);
                writer.Write(binary.byte3);
                writer.Write(binary.byte2);
                writer.Write(binary.byte1);
                writer.Write(binary.byte0);
            }
            else
            {
                writer.Write(binary.byte0);
                writer.Write(binary.byte1);
                writer.Write(binary.byte2);
                writer.Write(binary.byte3);
                writer.Write(binary.byte4);
                writer.Write(binary.byte5);
                writer.Write(binary.byte6);
                writer.Write(binary.byte7);
            }
        }

        double IMsgPackConverter<double>.Read(IMsgPackReader reader, MsgPackContext context, Func<double> creator)
        {
            var type = (DataTypes)reader.ReadByte();

            if (type != DataTypes.Single && type != DataTypes.Double)
                throw ExceptionUtils.BadTypeException(type, DataTypes.Single, DataTypes.Double);

            if (type == DataTypes.Single)
            {
                return ReadFloat(reader);
            }

            var bytes = ReadBytes(reader, 8);

            return new DoubleBinary(bytes).value;
        }

        public void Write(float value, IMsgPackWriter writer, MsgPackContext context)
        {
            var binary = new FloatBinary(value);
            writer.Write(DataTypes.Single);
            if (BitConverter.IsLittleEndian)
            {
                writer.Write(binary.byte3);
                writer.Write(binary.byte2);
                writer.Write(binary.byte1);
                writer.Write(binary.byte0);
            }
            else
            {
                writer.Write(binary.byte0);
                writer.Write(binary.byte1);
                writer.Write(binary.byte2);
                writer.Write(binary.byte3);
            }
        }

        float IMsgPackConverter<float>.Read(IMsgPackReader reader, MsgPackContext context, Func<float> creator)
        {
            var type = (DataTypes)reader.ReadByte();

            if (type != DataTypes.Single)
                throw ExceptionUtils.BadTypeException(type, DataTypes.Single);

            return ReadFloat(reader);
        }

        private static float ReadFloat(IMsgPackReader reader)
        {
            var bytes = ReadBytes(reader, 4);

            return new FloatBinary(bytes).value;
        }

        private static byte[] ReadBytes(IMsgPackReader reader, int length)
        {
            var bytes = new byte[length];
            reader.ReadBytes(bytes);
            return bytes;
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct FloatBinary
        {
            [FieldOffset(0)]
            public readonly float value;

            [FieldOffset(0)]
            public readonly byte byte0;

            [FieldOffset(1)]
            public readonly byte byte1;

            [FieldOffset(2)]
            public readonly byte byte2;

            [FieldOffset(3)]
            public readonly byte byte3;

            public FloatBinary(float f)
            {
                this = default(FloatBinary);
                value = f;
            }

            public FloatBinary(byte[] bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes[3];
                    byte1 = bytes[2];
                    byte2 = bytes[1];
                    byte3 = bytes[0];
                }
                else
                {
                    byte0 = bytes[0];
                    byte1 = bytes[1];
                    byte2 = bytes[2];
                    byte3 = bytes[3];
                }
            }
        }

        [StructLayout(LayoutKind.Explicit)]
        private struct DoubleBinary
        {
            [FieldOffset(0)]
            public readonly double value;

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

            public DoubleBinary(double f)
            {
                this = default(DoubleBinary);
                value = f;
            }

            public DoubleBinary(byte[] bytes)
            {
                value = 0;
                if (BitConverter.IsLittleEndian)
                {
                    byte0 = bytes[7];
                    byte1 = bytes[6];
                    byte2 = bytes[5];
                    byte3 = bytes[4];
                    byte4 = bytes[3];
                    byte5 = bytes[2];
                    byte6 = bytes[1];
                    byte7 = bytes[0];
                }
                else
                {
                    byte0 = bytes[0];
                    byte1 = bytes[1];
                    byte2 = bytes[2];
                    byte3 = bytes[3];
                    byte4 = bytes[4];
                    byte5 = bytes[5];
                    byte6 = bytes[6];
                    byte7 = bytes[7];
                }
            }
        }
    }
}