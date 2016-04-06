using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MsgPackLite
{
    public class MsgPackLite
    {
        #region Private Constants

        private const int Max4Bit = 0xf;
        private const int Max5Bit = 0x1f;
        private const int Max7Bit = 0x7f;
        private const int Max8Bit = 0xff;
        private const int Max15Bit = 0x7fff;
        private const int Max16Bit = 0xffff;
        private const int Max31Bit = 0x7fffffff;
        private const long Max32Bit = 0xffffffffL;

        //these values are from http://wiki.msgpack.org/display/MSGPACK/Format+specification
        private const byte MpNull = 0xc0;
        private const byte MpFalse = 0xc2;
        private const byte MpTrue = 0xc3;

        private const byte MpFloat = 0xca;
        private const byte MpDouble = 0xcb;

        private const byte MpUint8 = 0xcc;
        private const byte MpUint16 = 0xcd;
        private const byte MpUint32 = 0xce;
        private const byte MpUint64 = 0xcf;

        private const byte MpNegativeFixnum = 0xe0; //last 5 bits is value
        private const byte MpInt8 = 0xd0;
        private const byte MpInt16 = 0xd1;
        private const byte MpInt32 = 0xd2;
        private const byte MpInt64 = 0xd3;

        private const byte MpFixarray = 0x90; //last 4 bits is size
        private const byte MpArray16 = 0xdc;
        private const byte MpArray32 = 0xdd;

        private const byte MpFixmap = 0x80; //last 4 bits is size
        private const byte MpMap16 = 0xde;
        private const byte MpMap32 = 0xdf;

        private const byte MpFixstr = 0xa0; //last 5 bits is size
        private const byte MpStr8 = 0xd9;
        private const byte MpStr16 = 0xda;
        private const byte MpStr32 = 0xdb;

        private const byte MpBit8 = 0xc4;
        private const byte MpBit16 = 0xc5;
        private const byte MpBit32 = 0xc6;

        #endregion

        #region Public Methods

        public static object Unpack(byte[] bytesArray)
        {
            var stream = new MemoryStream(bytesArray);
            var reader = new BinaryReader(stream);
            return Unpack(reader);
        }

        public static byte[] Pack(object item)
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            Pack(item, writer);

            return stream.ToArray();
        }

        #endregion

        #region Private Methods

        #region Packing

        private static void Pack(object item, BinaryWriter writer)
        {
            if (item == null)
            {
                PackNull(writer);
            }
            else if (item is bool)
            {
                PackBoolean((bool)item, writer);
            }
            else if (item is float)
            {
                PackFloat((float)item, writer);
            }
            else if (item is double)
            {
                PackDouble((double)item, writer);
            }
            else if (IsIntegerNumber(item))
            {
                PackIntegerNumber(item, writer);
            }
            else if (item is string)
            {
                PackString((string)item, writer);
            }
            else if (item is byte[])
            {
                PackByteArray((byte[])item, writer);
            }
            else if (item is IList)
            {
                PackList((IList)item, writer);
            }
            else if (item is IDictionary)
            {
                PackMap((IDictionary)item, writer);
            }
            else
            {
                throw new ArgumentException("Cannot msgpack object of type " + item.GetType().FullName);
            }
        }

        private static void PackDouble(double item, BinaryWriter writer)
        {
            writer.Write(MpDouble);
            writer.Write(ToBigEndianBytes(item));
        }

        private static void PackFloat(float item, BinaryWriter writer)
        {
            writer.Write(MpFloat);
            writer.Write(ToBigEndianBytes(item));
        }

        private static void PackBoolean(bool item, BinaryWriter writer)
        {
            writer.Write(item ? MpTrue : MpFalse);
        }

        private static void PackNull(BinaryWriter writer)
        {
            writer.Write(MpNull);
        }

        private static void PackString(string item, BinaryWriter writer)
        {
            var data = Encoding.UTF8.GetBytes(item);

            if (data.Length <= Max5Bit)
            {
                writer.Write((byte)(data.Length | MpFixstr));
            }
            else if (data.Length <= Max8Bit)
            {
                writer.Write(MpStr8);
                writer.Write((byte)data.Length);
            }
            else if (data.Length <= Max16Bit)
            {
                writer.Write(MpStr16);
                writer.Write(ToBigEndianBytes((ushort)data.Length));
            }
            else
            {
                writer.Write(MpStr32);
                writer.Write(ToBigEndianBytes((uint)data.Length));
            }
            writer.Write(data);
        }

        private static void PackByteArray(byte[] data, BinaryWriter writer)
        {
            if (data.Length <= Max8Bit)
            {
                writer.Write(MpBit8);
                writer.Write((byte)data.Length);
            }
            else if (data.Length <= Max16Bit)
            {
                writer.Write(MpBit16);
                writer.Write(ToBigEndianBytes((ushort)data.Length));
            }
            else
            {
                writer.Write(MpBit32);
                writer.Write(ToBigEndianBytes((uint)data.Length));
            }

            writer.Write(data);
        }

        private static void PackMap(IDictionary item, BinaryWriter writer)
        {
            var map = (IDictionary<object, object>)item;
            if (map.Count <= Max4Bit)
            {
                writer.Write((byte)(map.Count | MpFixmap));
            }
            else if (map.Count <= Max16Bit)
            {
                writer.Write(MpMap16);
                writer.Write(ToBigEndianBytes((ushort)map.Count));
            }
            else
            {
                writer.Write(MpMap32);
                writer.Write(ToBigEndianBytes((uint)map.Count));
            }
            foreach (var kvp in map)
            {
                Pack(kvp.Key, writer);
                Pack(kvp.Value, writer);
            }
        }

        private static void PackList(ICollection list, BinaryWriter writer)
        {
            var length = list.Count;

            if (length <= Max4Bit)
            {
                writer.Write((byte)(length | MpFixarray));
            }
            else if (length <= Max16Bit)
            {
                writer.Write(MpArray16);
                writer.Write(ToBigEndianBytes((ushort)length));
            }
            else
            {
                writer.Write(MpArray32);
                writer.Write(ToBigEndianBytes((uint)length));
            }
            foreach (var element in list)
            {
                Pack(element, writer);
            }
        }

        private static void PackIntegerNumber(object item, BinaryWriter outputWriter)
        {
            if (item is ulong && (ulong)item > long.MaxValue)
            {
                outputWriter.Write(MpUint64);
                outputWriter.Write(ToBigEndianBytes((ulong)item));
            }
            else
            {
                var value = CastToLong(item);
                if (value >= 0)
                {
                    if (value <= Max7Bit)
                    {
                        outputWriter.Write((byte)value);
                    }
                    else if (value <= Max8Bit)
                    {
                        outputWriter.Write(MpUint8);
                        outputWriter.Write((byte)value);
                    }
                    else if (value <= Max16Bit)
                    {
                        outputWriter.Write(MpUint16);
                        outputWriter.Write(ToBigEndianBytes((ushort)value));
                    }
                    else if (value <= Max32Bit)
                    {
                        outputWriter.Write(MpUint32);
                        outputWriter.Write(ToBigEndianBytes((uint)value));
                    }
                    else
                    {
                        outputWriter.Write(MpUint64);
                        outputWriter.Write(ToBigEndianBytes(value));
                    }
                }
                else
                {
                    if (value >= -(Max5Bit + 1))
                    {
                        outputWriter.Write((byte)(value & 0xff));
                    }
                    else if (value >= -Max7Bit)
                    {
                        outputWriter.Write(MpInt8);
                        outputWriter.Write((byte)value);
                    }
                    else if (value >= -Max15Bit)
                    {
                        outputWriter.Write(MpInt16);
                        outputWriter.Write(ToBigEndianBytes((short)value));
                    }
                    else if (value >= -Max31Bit)
                    {
                        outputWriter.Write(MpInt32);
                        outputWriter.Write(ToBigEndianBytes((int)value));
                    }
                    else
                    {
                        outputWriter.Write(MpInt64);
                        outputWriter.Write(ToBigEndianBytes(value));
                    }
                }
            }
        }

        #endregion

        #region Unpacking

        private static object Unpack(BinaryReader reader)
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case MpNull:
                    return null;
                case MpFalse:
                    return false;
                case MpTrue:
                    return true;
                case MpFloat:
                    return ReadSingle(reader);
                case MpDouble:
                    return ReadDouble(reader);
                case MpUint8:
                    return reader.ReadByte();
                case MpUint16:
                    return ReadUInt16(reader);
                case MpUint32:
                    return ReadUInt32(reader);
                case MpUint64:
                    return ReadUInt64(reader);
                case MpInt8:
                    return reader.ReadSByte();
                case MpInt16:
                    return ReadInt16(reader);
                case MpInt32:
                    return ReadInt32(reader);
                case MpInt64:
                    return ReadInt64(reader);
                case MpArray16:
                    return UnpackList(ReadInt16(reader) & Max16Bit, reader);
                case MpArray32:
                    return UnpackList(ReadInt32(reader), reader);
                case MpMap16:
                    return UnpackMap(ReadInt16(reader) & Max16Bit, reader);
                case MpMap32:
                    return UnpackMap(ReadInt32(reader), reader);
                case MpStr8:
                    return UnpackString(reader.ReadByte() & Max8Bit, reader);
                case MpStr16:
                    return UnpackString(ReadInt16(reader) & Max16Bit, reader);
                case MpStr32:
                    return UnpackString(ReadInt32(reader), reader);
                case MpBit8:
                    return UnpackBin(reader.ReadByte() & Max8Bit, reader);
                case MpBit16:
                    return UnpackBin(ReadInt16(reader) & Max16Bit, reader);
                case MpBit32:
                    return UnpackBin(ReadInt32(reader), reader);
            }

            if ((value & MpNegativeFixnum) == MpNegativeFixnum)
                return (sbyte) value;

            if (value >= MpNegativeFixnum && value <= MpNegativeFixnum + Max5Bit)
            {
                return value;
            }

            if (value >= MpFixarray && value <= MpFixarray + Max4Bit)
            {
                return UnpackList(value - MpFixarray, reader);
            }

            if (value >= MpFixmap && value <= MpFixmap + Max4Bit)
            {
                return UnpackMap(value - MpFixmap, reader);
            }

            if (value >= MpFixstr && value <= MpFixstr + Max5Bit)
            {
                return UnpackString(value - MpFixstr, reader);
            }

            if (value <= Max7Bit)
            {
                //MP_FIXNUM - the value is value as an int
                return value;
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        private static byte[] UnpackBin(int size, BinaryReader reader)
        {
            if (size < 0)
            {
                throw new ArgumentException("byte[] to unpack too large (more than 2^31 elements)!");
            }

            var data = new byte[size];

            reader.Read(data, 0, size);

            return data;
        }

        private static string UnpackString(int size, BinaryReader reader)
        {
            if (size < 0)
            {
                throw new ArgumentException("byte[] to unpack too large (more than 2^31 elements)!");
            }

            var data = new byte[size];

            reader.Read(data, 0, size);

            return Encoding.UTF8.GetString(data);
        }

        private static IDictionary UnpackMap(int size, BinaryReader reader)
        {
            if (size < 0)
            {
                throw new ArgumentException("Map to unpack too large (more than 2^31 elements)!");
            }

            var ret = new Dictionary<object, object>(size);
            for (var i = 0; i < size; ++i)
            {
                var key = Unpack(reader);
                var value = Unpack(reader);
                ret.Add(key, value);
            }

            return ret;
        }

        private static IList UnpackList(int size, BinaryReader reader)
        {
            if (size < 0)
            {
                throw new ArgumentException("Array to unpack too large (more than 2^31 elements)!");
            }
            var ret = new object[size];

            for (var i = 0; i < size; ++i)
            {
                ret[i] = Unpack(reader);
            }

            return ret;
        }

        private static bool IsIntegerNumber(object value)
        {
            return value is sbyte
                   || value is byte
                   || value is short
                   || value is ushort
                   || value is int
                   || value is uint
                   || value is long
                   || value is ulong;
        }

        private static long CastToLong(object item)
        {
            if (item is sbyte)
                return (sbyte) item;
            if (item is byte)
                return (byte) item;
            if (item is short)
                return (short) item;
            if (item is ushort)
                return (ushort) item;
            if (item is int)
                return (int) item;
            if (item is uint)
                return (uint) item;
            if (item is long)
                return (long) item;
            if (item is ulong)
                return (long) (ulong) item;

            throw new ArgumentException("Item can't be converted to long :" + item);
        }

        #endregion

        #region Convert To/From Big Endian Methods

        private static byte[] ToBigEndianBytes(float item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(double item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(ushort item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(short item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(uint item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(int item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(ulong item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(long item)
        {
            return ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static long ReadInt64(BinaryReader reader)
        {
            return BitConverter.ToInt64(ReverseArrayIfNeeded(reader.ReadBytes(8)), 0);
        }

        private static int ReadInt32(BinaryReader reader)
        {
            return BitConverter.ToInt32(ReverseArrayIfNeeded(reader.ReadBytes(4)), 0);
        }

        private static short ReadInt16(BinaryReader reader)
        {
            return BitConverter.ToInt16(ReverseArrayIfNeeded(reader.ReadBytes(2)), 0);
        }

        private static ulong ReadUInt64(BinaryReader reader)
        {
            return BitConverter.ToUInt64(ReverseArrayIfNeeded(reader.ReadBytes(8)), 0);
        }

        private static uint ReadUInt32(BinaryReader reader)
        {
            return BitConverter.ToUInt32(ReverseArrayIfNeeded(reader.ReadBytes(4)), 0);
        }

        private static ushort ReadUInt16(BinaryReader reader)
        {
            return BitConverter.ToUInt16(ReverseArrayIfNeeded(reader.ReadBytes(2)), 0);
        }

        private static double ReadDouble(BinaryReader reader)
        {
            return BitConverter.ToDouble(ReverseArrayIfNeeded(reader.ReadBytes(8)), 0);
        }

        private static float ReadSingle(BinaryReader reader)
        {
            return BitConverter.ToSingle(ReverseArrayIfNeeded(reader.ReadBytes(4)), 0);
        }

        private static byte[] ReverseArrayIfNeeded(byte[] array)
        {
            if (!BitConverter.IsLittleEndian)
                return array;

            var result = new byte[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                result[array.Length - 1 - i] = array[i];
            }

            return result;
        }
        #endregion

        #endregion
    }
}