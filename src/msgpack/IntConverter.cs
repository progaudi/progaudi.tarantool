using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class IntConverter :
        IMsgPackConverter<byte>, IMsgPackConverter<sbyte>,
        IMsgPackConverter<short>, IMsgPackConverter<ushort>,
        IMsgPackConverter<int>, IMsgPackConverter<uint>,
        IMsgPackConverter<long>, IMsgPackConverter<ulong>
    {
        public void Write(byte value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum(value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue(value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(sbyte value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum(value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue(value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(short value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                case IntFormatType.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;
                case IntFormatType.Int16:
                    WriteMPackValue(value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(ushort value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                case IntFormatType.UInt16:
                    WriteMPackValue(value, stream);
                    break;
                case IntFormatType.Int16:
                    WriteMPackValue((short)value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(int value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                case IntFormatType.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;
                case IntFormatType.Int16:
                    WriteMPackValue((short)value, stream);
                    break;
                case IntFormatType.UInt32:
                    WriteMPackValue((uint)value, stream);
                    break;
                case IntFormatType.Int32:
                    WriteMPackValue(value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(uint value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                case IntFormatType.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;
                case IntFormatType.Int16:
                    WriteMPackValue((short)value, stream);
                    break;
                case IntFormatType.UInt32:
                    WriteMPackValue(value, stream);
                    break;
                case IntFormatType.Int32:
                    WriteMPackValue((int)value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(long value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                case IntFormatType.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;
                case IntFormatType.Int16:
                    WriteMPackValue((short)value, stream);
                    break;
                case IntFormatType.UInt32:
                    WriteMPackValue((uint)value, stream);
                    break;
                case IntFormatType.Int32:
                    WriteMPackValue((int)value, stream);
                    break;
                case IntFormatType.UInt64:
                    WriteMPackValue((ulong)value, stream);
                    break;
                case IntFormatType.Int64:
                    WriteMPackValue(value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public void Write(ulong value, Stream stream, MsgPackSettings settings)
        {
            switch (value.GetFormatType())
            {
                case IntFormatType.PositiveFixNum:
                    WritePositiveFixNum((byte)value, stream);
                    break;
                case IntFormatType.NegativeFixNum:
                    WriteNegativeFixNum((sbyte)value, stream);
                    break;
                case IntFormatType.UInt8:
                    WriteMPackValue((byte)value, stream);
                    break;
                case IntFormatType.Int8:
                    WriteMPackValue((sbyte)value, stream);
                    break;
                case IntFormatType.UInt16:
                    WriteMPackValue((ushort)value, stream);
                    break;
                case IntFormatType.Int16:
                    WriteMPackValue((short)value, stream);
                    break;
                case IntFormatType.UInt32:
                    WriteMPackValue((uint)value, stream);
                    break;
                case IntFormatType.Int32:
                    WriteMPackValue((int)value, stream);
                    break;
                case IntFormatType.UInt64:
                    WriteMPackValue(value, stream);
                    break;
                case IntFormatType.Int64:
                    WriteMPackValue((long)value, stream);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
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
            var value = item >= 0 ? item : (1 << 8) + item;
            stream.WriteByte((byte)(value % 256));
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

        private static void WriteMPackValue(short item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int16);
            WriteValue(item, stream);
        }

        internal static void WriteValue(short item, Stream stream)
        {
            var value = item >= 0 ? item : ushort.MaxValue + item + 1;
            stream.WriteByte((byte)((value >> 8) % 256));
            stream.WriteByte((byte)(value % 256));
        }

        private static void WriteMPackValue(int item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int32);
            WriteValue(item, stream);
        }

        internal static void WriteValue(int item, Stream stream)
        {
            var value = item > 0 ? item : uint.MaxValue + item + 1;
            stream.WriteByte((byte)((value >> 24) % 256));
            stream.WriteByte((byte)((value >> 16) % 256));
            stream.WriteByte((byte)((value >> 8) % 256));
            stream.WriteByte((byte)(value % 256));
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

        private static void WriteMPackValue(long item, Stream stream)
        {
            stream.WriteByte((byte)DataTypes.Int64);
            WriteValue(item, stream);
        }

        internal static void WriteValue(long item, Stream stream)
        {
            var value = item >= 0 ? (ulong)item : ulong.MaxValue + (ulong)item + 1L;
            stream.WriteByte((byte)((value >> 56) % 256));
            stream.WriteByte((byte)((value >> 48) % 256));
            stream.WriteByte((byte)((value >> 40) % 256));
            stream.WriteByte((byte)((value >> 32) % 256));
            stream.WriteByte((byte)((value >> 24) % 256));
            stream.WriteByte((byte)((value >> 16) % 256));
            stream.WriteByte((byte)((value >> 8) % 256));
            stream.WriteByte((byte)(value % 256));
        }
    }
}