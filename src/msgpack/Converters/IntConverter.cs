using System;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class IntConverter :
        IMsgPackConverter<byte>,
        IMsgPackConverter<sbyte>,
        IMsgPackConverter<short>,
        IMsgPackConverter<ushort>,
        IMsgPackConverter<int>,
        IMsgPackConverter<uint>,
        IMsgPackConverter<long>,
        IMsgPackConverter<ulong>
    {
        public void Write(byte value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum(value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue(value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public byte Read(IBytesReader reader, MsgPackContext context, Func<byte> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return (byte)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.Int8:
                    return (byte)ReadInt8(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(int value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, writer);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, writer);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue((uint)value, writer);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue(value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public int Read(IBytesReader reader, MsgPackContext context, Func<int> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.Int8:
                    return ReadInt8(reader);

                case DataTypes.Int16:
                    return ReadInt16(reader);

                case DataTypes.Int32:
                    return ReadInt32(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(long value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, writer);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, writer);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue((uint)value, writer);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue((int)value, writer);
                    break;

                case DataTypes.UInt64:
                    WriteMPackValue((ulong)value, writer);
                    break;

                case DataTypes.Int64:
                    WriteMPackValue(value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public long Read(IBytesReader reader, MsgPackContext context, Func<long> creator)
        {
            var type = reader.ReadDataType();

            byte tempUInt8;
            if (TryGetFixPositiveNumber(type, out tempUInt8))
            {
                return tempUInt8;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    return ReadUInt32(reader);

                case DataTypes.Int8:
                    return ReadInt8(reader);

                case DataTypes.Int16:
                    return ReadInt16(reader);

                case DataTypes.Int32:
                    return ReadInt32(reader);

                case DataTypes.Int64:
                    return ReadInt64(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(sbyte value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum(value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue(value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public sbyte Read(IBytesReader reader, MsgPackContext context, Func<sbyte> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return (sbyte)temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return tempInt8;
            }

            if (type == DataTypes.Int8)
            {
                return ReadInt8(reader);
            }

            throw ExceptionUtils.IntDeserializationFailure(type);
        }
        
        public void Write(short value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, writer);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue(value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public short Read(IBytesReader reader, MsgPackContext context, Func<short> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.Int8:
                    return ReadInt8(reader);

                case DataTypes.Int16:
                    return ReadInt16(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(uint value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, writer);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, writer);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue(value, writer);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue((int)value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public uint Read(IBytesReader reader, MsgPackContext context, Func<uint> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return (uint)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    return ReadUInt32(reader);

                case DataTypes.Int8:
                    return (uint)ReadInt8(reader);

                case DataTypes.Int16:
                    return (uint)ReadInt16(reader);

                case DataTypes.Int32:
                    return (uint)ReadInt32(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }

        public void Write(ulong value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, writer);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, writer);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue((uint)value, writer);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue((int)value, writer);
                    break;

                case DataTypes.UInt64:
                    WriteMPackValue(value, writer);
                    break;

                case DataTypes.Int64:
                    WriteMPackValue((long)value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ulong Read(IBytesReader reader, MsgPackContext context, Func<ulong> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return (ulong)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.UInt32:
                    return ReadUInt32(reader);

                case DataTypes.UInt64:
                    return ReadUInt64(reader);

                case DataTypes.Int8:
                    return (ulong)ReadInt8(reader);

                case DataTypes.Int16:
                    return (ulong)ReadInt16(reader);

                case DataTypes.Int32:
                    return (ulong)ReadInt32(reader);

                case DataTypes.Int64:
                    return (ulong)ReadInt64(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }
        
        public void Write(ushort value, IBytesWriter writer, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, writer);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, writer);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, writer);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, writer);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue(value, writer);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, writer);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public ushort Read(IBytesReader reader, MsgPackContext context, Func<ushort> creator)
        {
            var type = reader.ReadDataType();

            byte temp;
            if (TryGetFixPositiveNumber(type, out temp))
            {
                return temp;
            }

            sbyte tempInt8;
            if (TryGetNegativeNumber(type, out tempInt8))
            {
                return (ushort)tempInt8;
            }

            switch (type)
            {
                case DataTypes.UInt8:
                    return ReadUInt8(reader);

                case DataTypes.UInt16:
                    return ReadUInt16(reader);

                case DataTypes.Int8:
                    return (ushort)ReadInt8(reader);

                case DataTypes.Int16:
                    return (ushort)ReadInt16(reader);

                default:
                    throw ExceptionUtils.IntDeserializationFailure(type);
            }
        }
        
        private bool TryGetFixPositiveNumber(DataTypes type, out byte temp)
        {
            temp = (byte)type;
            return type.GetHighBits(1) == DataTypes.PositiveFixNum.GetHighBits(1);
        }

        private bool TryGetNegativeNumber(DataTypes type, out sbyte temp)
        {
            temp = (sbyte)((byte)type - 1 - byte.MaxValue);

            return type.GetHighBits(3) == DataTypes.NegativeFixNum.GetHighBits(3);
        }

        private static void WriteNegativeFixNum(sbyte item, IBytesWriter writer)
        {
            writer.Write((byte)(byte.MaxValue + item + 1));
        }

        private static void WritePositiveFixNum(byte item, IBytesWriter writer)
        {
            writer.Write(item);
        }

        private static void WriteMPackValue(sbyte item, IBytesWriter writer)
        {
            writer.Write(DataTypes.Int8);
            WriteValue(item, writer);
        }

        internal static void WriteValue(sbyte item, IBytesWriter writer)
        {
            var value = item >= 0 ? item : byte.MaxValue + item + 1;
            writer.Write((byte)(value % 256));
        }

        internal static sbyte ReadInt8(IBytesReader reader)
        {
            var temp = reader.ReadByte();
            if (temp <= sbyte.MaxValue)
                return (sbyte)temp;

            return (sbyte)(temp - byte.MaxValue - 1);
        }

        private static void WriteMPackValue(byte item, IBytesWriter writer)
        {
            writer.Write(DataTypes.UInt8);
            WriteValue(item, writer);
        }

        internal static void WriteValue(byte item, IBytesWriter writer)
        {
            writer.Write(item);
        }

        internal static byte ReadUInt8(IBytesReader reader)
        {
            return reader.ReadByte();
        }

        private static void WriteMPackValue(ushort item, IBytesWriter writer)
        {
            writer.Write(DataTypes.UInt16);
            WriteValue(item, writer);
        }

        internal static void WriteValue(ushort item, IBytesWriter writer)
        {
            writer.Write((byte)((item >> 8) % 256));
            writer.Write((byte)(item % 256));
        }

        internal static ushort ReadUInt16(IBytesReader reader)
        {
            return (ushort)((reader.ReadByte() << 8) + reader.ReadByte());
        }

        private static void WriteMPackValue(short item, IBytesWriter writer)
        {
            writer.Write(DataTypes.Int16);
            WriteValue(item, writer);
        }

        internal static void WriteValue(short item, IBytesWriter writer)
        {
            var value = (ushort)(item >= 0 ? item : ushort.MaxValue + item + 1);
            WriteValue(value, writer);
        }

        internal static short ReadInt16(IBytesReader reader)
        {
            var temp = ReadUInt16(reader);
            if (temp <= short.MaxValue)
                return (short)temp;

            return (short)(temp - 1 - ushort.MaxValue);
        }

        private static void WriteMPackValue(int item, IBytesWriter writer)
        {
            writer.Write(DataTypes.Int32);
            WriteValue(item, writer);
        }

        internal static void WriteValue(int item, IBytesWriter writer)
        {
            var value = (uint)(item > 0 ? item : uint.MaxValue + item + 1);
            WriteValue(value, writer);
        }

        internal static int ReadInt32(IBytesReader reader)
        {
            var temp = ReadUInt32(reader);
            if (temp <= int.MaxValue)
                return (int)temp;

            return (int)(temp - 1 - uint.MaxValue);
        }

        private static void WriteMPackValue(uint item, IBytesWriter writer)
        {
            writer.Write(DataTypes.UInt32);
            WriteValue(item, writer);
        }

        internal static void WriteValue(uint item, IBytesWriter writer)
        {
            writer.Write((byte)((item >> 24) % 256));
            writer.Write((byte)((item >> 16) % 256));
            writer.Write((byte)((item >> 8) % 256));
            writer.Write((byte)(item % 256));
        }

        internal static uint ReadUInt32(IBytesReader reader)
        {
            var temp = (uint)(reader.ReadByte() << 24);
            temp += (uint)reader.ReadByte() << 16;
            temp += (uint)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }

        private static void WriteMPackValue(ulong item, IBytesWriter writer)
        {
            writer.Write(DataTypes.UInt64);
            WriteValue(item, writer);
        }

        internal static void WriteValue(ulong item, IBytesWriter writer)
        {
            writer.Write((byte)((item >> 56) % 256));
            writer.Write((byte)((item >> 48) % 256));
            writer.Write((byte)((item >> 40) % 256));
            writer.Write((byte)((item >> 32) % 256));
            writer.Write((byte)((item >> 24) % 256));
            writer.Write((byte)((item >> 16) % 256));
            writer.Write((byte)((item >> 8) % 256));
            writer.Write((byte)(item % 256));
        }

        internal static ulong ReadUInt64(IBytesReader reader)
        {
            var temp = (ulong)reader.ReadByte() << 56;
            temp += (ulong)reader.ReadByte() << 48;
            temp += (ulong)reader.ReadByte() << 40;
            temp += (ulong)reader.ReadByte() << 32;
            temp += (ulong)reader.ReadByte() << 24;
            temp += (ulong)reader.ReadByte() << 16;
            temp += (ulong)reader.ReadByte() << 8;
            temp += reader.ReadByte();

            return temp;
        }

        private static void WriteMPackValue(long item, IBytesWriter writer)
        {
            writer.Write(DataTypes.Int64);
            WriteValue(item, writer);
        }

        internal static void WriteValue(long item, IBytesWriter writer)
        {
            var value = item >= 0 ? (ulong)item : ulong.MaxValue + (ulong)item + 1L;
            WriteValue(value, writer);
        }

        internal static long ReadInt64(IBytesReader reader)
        {
            var temp = ReadUInt64(reader);
            if (temp <= long.MaxValue)
                return (long)temp;

            return (long)(temp - 1 - ulong.MaxValue);
        }
    }
}