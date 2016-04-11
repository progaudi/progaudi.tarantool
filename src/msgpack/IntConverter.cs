using System;
using System.IO;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
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
        public void Write(byte value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum(value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue(value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        byte IMsgPackConverter<byte>.Read(Stream stream, MsgPackContext context, Func<byte> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.Int8:
                    return (byte)ReadInt8(stream);

                default:
                    throw new SerializationException($"Waited for an int, got {type:G} (0x{type:X})");
            }
        }

        public void Write(int value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, stream);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue((uint)value, stream);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue(value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        int IMsgPackConverter<int>.Read(Stream stream, MsgPackContext context, Func<int> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.UInt16:
                    return ReadUInt16(stream);

                case DataTypes.Int8:
                    return ReadInt8(stream);

                case DataTypes.Int16:
                    return ReadInt16(stream);

                case DataTypes.Int32:
                    return ReadInt32(stream);

                default:
                    throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
            }
        }

        public void Write(long value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, stream);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue((uint)value, stream);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue((int)value, stream);
                    break;

                case DataTypes.UInt64:
                    WriteMPackValue((ulong)value, stream);
                    break;

                case DataTypes.Int64:
                    WriteMPackValue(value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        long IMsgPackConverter<long>.Read(Stream stream, MsgPackContext context, Func<long> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.UInt16:
                    return ReadUInt16(stream);

                case DataTypes.UInt32:
                    return ReadUInt32(stream);

                case DataTypes.Int8:
                    return ReadInt8(stream);

                case DataTypes.Int16:
                    return ReadInt16(stream);

                case DataTypes.Int32:
                    return ReadInt32(stream);

                case DataTypes.Int64:
                    return ReadInt64(stream);

                default:
                    throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
            }
        }

        public void Write(sbyte value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum(value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue(value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        sbyte IMsgPackConverter<sbyte>.Read(Stream stream, MsgPackContext context, Func<sbyte> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                return ReadInt8(stream);
            }

            throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
        }

        public void Write(short value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue(value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        short IMsgPackConverter<short>.Read(Stream stream, MsgPackContext context, Func<short> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.Int8:
                    return ReadInt8(stream);

                case DataTypes.Int16:
                    return ReadInt16(stream);

                default:
                    throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
            }
        }

        public void Write(uint value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, stream);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue(value, stream);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue((int)value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        uint IMsgPackConverter<uint>.Read(Stream stream, MsgPackContext context, Func<uint> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.UInt16:
                    return ReadUInt16(stream);

                case DataTypes.UInt32:
                    return ReadUInt32(stream);

                case DataTypes.Int8:
                    return (uint)ReadInt8(stream);

                case DataTypes.Int16:
                    return (uint)ReadInt16(stream);

                case DataTypes.Int32:
                    return (uint)ReadInt32(stream);

                default:
                    throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
            }
        }

        public void Write(ulong value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, stream);
                    break;

                case DataTypes.UInt32:
                    WriteMPackValue((uint)value, stream);
                    break;

                case DataTypes.Int32:
                    WriteMPackValue((int)value, stream);
                    break;

                case DataTypes.UInt64:
                    WriteMPackValue(value, stream);
                    break;

                case DataTypes.Int64:
                    WriteMPackValue((long)value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ulong IMsgPackConverter<ulong>.Read(Stream stream, MsgPackContext context, Func<ulong> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.UInt16:
                    return ReadUInt16(stream);

                case DataTypes.UInt32:
                    return ReadUInt32(stream);

                case DataTypes.UInt64:
                    return ReadUInt64(stream);

                case DataTypes.Int8:
                    return (ulong)ReadInt8(stream);

                case DataTypes.Int16:
                    return (ulong)ReadInt16(stream);

                case DataTypes.Int32:
                    return (ulong)ReadInt32(stream);

                case DataTypes.Int64:
                    return (ulong)ReadInt64(stream);

                default:
                    throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
            }
        }

        public void Write(ushort value, Stream stream, MsgPackContext context)
        {
            switch (value.GetFormatType())
            {
                case DataTypes.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;

                case DataTypes.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;

                case DataTypes.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;

                case DataTypes.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;

                case DataTypes.UInt16:
                    WriteMPackValue(value, stream);
                    break;

                case DataTypes.Int16:
                    WriteMPackValue((short)value, stream);
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        ushort IMsgPackConverter<ushort>.Read(Stream stream, MsgPackContext context, Func<ushort> creator)
        {
            var type = (DataTypes)stream.ReadByte();

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
                    return ReadUInt8(stream);

                case DataTypes.UInt16:
                    return ReadUInt16(stream);

                case DataTypes.Int8:
                    return (ushort)ReadInt8(stream);

                case DataTypes.Int16:
                    return (ushort)ReadInt16(stream);

                default:
                    throw new SerializationException($"Waited for an int, got ${type:G} (0x{type:X})");
            }
        }

        private bool TryGetFixPositiveNumber(DataTypes type, out byte temp)
        {
            if ((type & DataTypes.PositiveFixNum) == type)
            {
                temp = (byte)type;
                return true;
            }

            temp = 0;
            return false;
        }

        private bool TryGetNegativeNumber(DataTypes type, out sbyte temp)
        {
            if ((type & DataTypes.NegativeFixNum) == DataTypes.NegativeFixNum)
            {
                temp = (sbyte)((byte)type - 1 - byte.MaxValue);
                return true;
            }

            temp = 0;
            return false;
        }

        private static void WriteNegativeFixNum(sbyte item, Stream stream)
        {
            stream.WriteByte((byte)(byte.MaxValue + item + 1));
        }

        private static void WritePositiveFixNum(byte item, Stream stream)
        {
            stream.WriteByte(item);
        }

        private static void WriteMPackValue(sbyte item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int8);
            WriteValue(item, stream);
        }

        internal static void WriteValue(sbyte item, Stream stream)
        {
            var value = item >= 0 ? item : byte.MaxValue + item + 1;
            stream.WriteByte((byte)(value % 256));
        }

        internal static sbyte ReadInt8(Stream stream)
        {
            var temp = (byte)stream.ReadByte();
            if (temp <= sbyte.MaxValue)
                return (sbyte)temp;

            return (sbyte)(temp - byte.MaxValue - 1);
        }

        private static void WriteMPackValue(byte item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.UInt8);
            WriteValue(item, stream);
        }

        internal static void WriteValue(byte item, Stream stream)
        {
            stream.WriteByte(item);
        }

        internal static byte ReadUInt8(Stream stream)
        {
            return (byte)stream.ReadByte();
        }

        private static void WriteMPackValue(ushort item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.UInt16);
            WriteValue(item, stream);
        }

        internal static void WriteValue(ushort item, Stream stream)
        {
            stream.WriteByte((byte)((item >> 8) % 256));
            stream.WriteByte((byte)(item % 256));
        }

        internal static ushort ReadUInt16(Stream stream)
        {
            return (ushort)((stream.ReadByte() << 8) + stream.ReadByte());
        }

        private static void WriteMPackValue(short item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int16);
            WriteValue(item, stream);
        }

        internal static void WriteValue(short item, Stream stream)
        {
            var value = (ushort)(item >= 0 ? item : ushort.MaxValue + item + 1);
            WriteValue(value, stream);
        }

        internal static short ReadInt16(Stream stream)
        {
            var temp = ReadUInt16(stream);
            if (temp <= short.MaxValue)
                return (short)temp;

            return (short)(temp - 1 - ushort.MaxValue);
        }

        private static void WriteMPackValue(int item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int32);
            WriteValue(item, stream);
        }

        internal static void WriteValue(int item, Stream stream)
        {
            var value = (uint)(item > 0 ? item : uint.MaxValue + item + 1);
            WriteValue(value, stream);
        }

        internal static int ReadInt32(Stream stream)
        {
            var temp = ReadUInt32(stream);
            if (temp <= int.MaxValue)
                return (int)temp;

            return (int)(temp - 1 - uint.MaxValue);
        }

        private static void WriteMPackValue(uint item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.UInt32);
            WriteValue(item, stream);
        }

        internal static void WriteValue(uint item, Stream stream)
        {
            stream.WriteByte((byte)((item >> 24) % 256));
            stream.WriteByte((byte)((item >> 16) % 256));
            stream.WriteByte((byte)((item >> 8) % 256));
            stream.WriteByte((byte)(item % 256));
        }

        internal static uint ReadUInt32(Stream stream)
        {
            var temp = (uint)(stream.ReadByte() << 24);
            temp += (uint)stream.ReadByte() << 16;
            temp += (uint)stream.ReadByte() << 8;
            temp += (uint)stream.ReadByte();

            return temp;
        }

        private static void WriteMPackValue(ulong item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.UInt64);
            WriteValue(item, stream);
        }

        internal static void WriteValue(ulong item, Stream stream)
        {
            stream.WriteByte((byte)((item >> 56) % 256));
            stream.WriteByte((byte)((item >> 48) % 256));
            stream.WriteByte((byte)((item >> 40) % 256));
            stream.WriteByte((byte)((item >> 32) % 256));
            stream.WriteByte((byte)((item >> 24) % 256));
            stream.WriteByte((byte)((item >> 16) % 256));
            stream.WriteByte((byte)((item >> 8) % 256));
            stream.WriteByte((byte)(item % 256));
        }

        internal static ulong ReadUInt64(Stream stream)
        {
            var temp = (ulong)stream.ReadByte() << 56;
            temp += (ulong)stream.ReadByte() << 48;
            temp += (ulong)stream.ReadByte() << 40;
            temp += (ulong)stream.ReadByte() << 32;
            temp += (ulong)stream.ReadByte() << 24;
            temp += (ulong)stream.ReadByte() << 16;
            temp += (ulong)stream.ReadByte() << 8;
            temp += (ulong)stream.ReadByte();

            return temp;
        }

        private static void WriteMPackValue(long item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int64);
            WriteValue(item, stream);
        }

        internal static void WriteValue(long item, Stream stream)
        {
            var value = item >= 0 ? (ulong)item : ulong.MaxValue + (ulong)item + 1L;
            WriteValue(value, stream);
        }

        internal static long ReadInt64(Stream stream)
        {
            var temp = ReadUInt64(stream);
            if (temp <= long.MaxValue)
                return (long)temp;

            return (long)(temp - 1 - ulong.MaxValue);
        }
    }
}