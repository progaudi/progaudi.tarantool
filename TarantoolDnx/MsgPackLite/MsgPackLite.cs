using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace MsgPackLite
{
    public class MsgPackLite
    {
        public const int OPTION_UNPACK_RAW_AS_STRING = 0x1;
        public const int OPTION_UNPACK_RAW_AS_BYTE_BUFFER = 0x2;

        private const int MAX_4BIT = 0xf;
        private const int MAX_5BIT = 0x1f;
        private const int MAX_7BIT = 0x7f;
        private const int MAX_8BIT = 0xff;
        private const int MAX_15BIT = 0x7fff;
        private const int MAX_16BIT = 0xffff;
        private const int MAX_31BIT = 0x7fffffff;
        private const long MAX_32BIT = 0xffffffffL;

        //these values are from http://wiki.msgpack.org/display/MSGPACK/Format+specification
        private const byte MP_NULL = (byte)0xc0;
        private const byte MP_FALSE = (byte)0xc2;
        private const byte MP_TRUE = (byte)0xc3;

        private const byte MP_FLOAT = (byte)0xca;
        private const byte MP_DOUBLE = (byte)0xcb;

        private const byte MP_FIXNUM = (byte)0x00;//last 7 bits is value
        private const byte MP_UINT8 = (byte)0xcc;
        private const byte MP_UINT16 = (byte)0xcd;
        private const byte MP_UINT32 = (byte)0xce;
        private const byte MP_UINT64 = (byte)0xcf;

        private const byte MP_NEGATIVE_FIXNUM = (byte)0xe0;//last 5 bits is value
        private const int MP_NEGATIVE_FIXNUM_INT = 0xe0;//  /me wishes for signed numbers.
        private const byte MP_INT8 = (byte)0xd0;
        private const byte MP_INT16 = (byte)0xd1;
        private const byte MP_INT32 = (byte)0xd2;
        private const byte MP_INT64 = (byte)0xd3;

        private const byte MP_FIXARRAY = (byte)0x90;//last 4 bits is size
        private const int MP_FIXARRAY_INT = 0x90;
        private const byte MP_ARRAY16 = (byte)0xdc;
        private const byte MP_ARRAY32 = (byte)0xdd;

        private const byte MP_FIXMAP = (byte)0x80;//last 4 bits is size
        private const int MP_FIXMAP_INT = 0x80;
        private const byte MP_MAP16 = (byte)0xde;
        private const byte MP_MAP32 = (byte)0xdf;

        private const byte MP_FIXRAW = (byte)0xa0;//last 5 bits is size
        private const int MP_FIXRAW_INT = 0xa0;
        private const byte MP_RAW8 = (byte)0xd9;
        private const byte MP_RAW16 = (byte)0xda;
        private const byte MP_RAW32 = (byte)0xdb;

        public static object Unpack(byte[] bytesArray, int options = 0)
        {
            var stream = new MemoryStream(bytesArray);
            var reader = new BinaryReader(stream);
            return Unpack(reader, options);
        }

        public static byte[] Pack(object item)
        {
            var stream = new MemoryStream();
            var writer = new BinaryWriter(stream);
            Pack(item, writer);

            return stream.ToArray();
        }

        private static void Pack(object item, BinaryWriter writer)
        {
            if (item == null)
            {
                writer.Write(MP_NULL);
            }
            else if (item is bool)
            {
                writer.Write((bool)item ? MP_TRUE : MP_FALSE);
            }
            else if (item is float)
            {
                writer.Write(MP_FLOAT);
                writer.Write(ToBigEndianBytes((float)item));
            }
            else if (item is double)
            {
                writer.Write(MP_DOUBLE);
                writer.Write(ToBigEndianBytes((double)item));
            }
            else if (IsIntegerNumber(item))
            {
                PackIntegerNumber(item, writer);
            }
            else if (item is string || item is byte[] || item is MemoryStream)
            {
                byte[] data;
                var s = item as string;

                if (s != null)
                {
                    data = System.Text.Encoding.UTF8.GetBytes(s);
                }
                else if (item is byte[])
                {
                    data = (byte[])item;
                }
                else
                {
                    var memoryStream = ((MemoryStream)item);
                    data = memoryStream.ToArray();
                }

                PackByteArray(writer, data);
            }
            else
            {
                var list1 = item as IList;
                if (list1 != null)
                {
                    PackList(writer, list1);
                }
                else if (item is IDictionary)
                {
                    PackMap(item, writer);
                }
                else
                {
                    throw new ArgumentException("Cannot msgpack object of type " + item.GetType().FullName);
                }
            }
        }

        private static byte[] ToBigEndianBytes(float item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(double item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(ushort item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(short item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(uint item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }
        private static byte[] ToBigEndianBytes(int item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(ulong item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(long item)
        {
            return ReverseArray(BitConverter.GetBytes(item));
        }

        private static byte[] ReverseArray(byte[] array)
        {
            if(!BitConverter.IsLittleEndian)
                return array;
            
            var result = new byte[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                result[array.Length - 1 - i] = array[i];
            }

            return result;
        }

        private static void PackByteArray(BinaryWriter writer, byte[] data)
        {
            if (data.Length <= MAX_5BIT)
            {
                writer.Write((byte) (data.Length | MP_FIXRAW));
            }
            else if (data.Length <= MAX_8BIT)
            {
                writer.Write(MP_RAW8);
                writer.Write((byte) data.Length);
            }
            else if (data.Length <= MAX_16BIT)
            {
                writer.Write(MP_RAW16);
                writer.Write(ToBigEndianBytes((ushort) data.Length));
            }
            else
            {
                writer.Write(MP_RAW32);
                writer.Write(ToBigEndianBytes((uint) data.Length));
            }
            writer.Write(data);
        }

        private static void PackMap(object item, BinaryWriter writer)
        {
            var map = (IDictionary<object, object>) item;
            if (map.Count <= MAX_4BIT)
            {
                writer.Write((byte) (map.Count | MP_FIXMAP));
            }
            else if (map.Count <= MAX_16BIT)
            {
                writer.Write(MP_MAP16);
                writer.Write(ToBigEndianBytes((ushort) map.Count));
            }
            else
            {
                writer.Write(MP_MAP32);
                writer.Write(ToBigEndianBytes((uint) map.Count));
            }
            foreach (var kvp in map)
            {
                Pack(kvp.Key, writer);
                Pack(kvp.Value, writer);
            }
        }

        private static void PackList(BinaryWriter writer, IList list1)
        {
            var list = list1;
            var length = list.Count;

            if (length <= MAX_4BIT)
            {
                writer.Write((byte) (length | MP_FIXARRAY));
            }
            else if (length <= MAX_16BIT)
            {
                writer.Write(MP_ARRAY16);
                writer.Write(ToBigEndianBytes((ushort) length));
            }
            else
            {
                writer.Write(MP_ARRAY32);
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
                outputWriter.Write(MP_UINT64);
                outputWriter.Write(ToBigEndianBytes((ulong)item));
            }
            else
            {
                var value = CastToLong(item);
                if (value >= 0)
                {
                    if (value <= MAX_7BIT)
                    {
                        outputWriter.Write((byte)value);
                    }
                    else if (value <= MAX_8BIT)
                    {
                        outputWriter.Write(MP_UINT8);
                        outputWriter.Write((byte)value);
                    }
                    else if (value <= MAX_16BIT)
                    {
                        outputWriter.Write(MP_UINT16);
                        outputWriter.Write(ToBigEndianBytes((ushort)value));
                    }
                    else if (value <= MAX_32BIT)
                    {
                        outputWriter.Write(MP_UINT32);
                        outputWriter.Write(ToBigEndianBytes((uint)value));
                    }
                    else
                    {
                        outputWriter.Write(MP_UINT64);
                        outputWriter.Write(ToBigEndianBytes(value));
                    }
                }
                else
                {
                    if (value >= -(MAX_5BIT + 1))
                    {
                        outputWriter.Write((byte)(value & 0xff));
                    }
                    else if (value >= -MAX_7BIT)
                    {
                        outputWriter.Write(MP_INT8);
                        outputWriter.Write((byte)value);
                    }
                    else if (value >= -MAX_15BIT)
                    {
                        outputWriter.Write(MP_INT16);
                        outputWriter.Write(ToBigEndianBytes((short)value));
                    }
                    else if (value >= -MAX_31BIT)
                    {
                        outputWriter.Write(MP_INT32);
                        outputWriter.Write(ToBigEndianBytes((int)value));
                    }
                    else
                    {
                        outputWriter.Write(MP_INT64);
                        outputWriter.Write(ToBigEndianBytes(value));
                    }
                }
            }
        }

        private static object Unpack(BinaryReader reader, int options)
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case MP_NULL:
                    return null;
                case MP_FALSE:
                    return false;
                case MP_TRUE:
                    return true;
                case MP_FLOAT:
                    return ReadSingle(reader);
                case MP_DOUBLE:
                    return ReadDouble(reader);
                case MP_UINT8:
                    return reader.ReadByte();
                case MP_UINT16:
                    return ReadUInt16(reader);
                case MP_UINT32:
                    return ReadUInt32(reader);
                case MP_UINT64:
                    return ReadUInt64(reader);
                case MP_INT8:
                    return reader.ReadByte();
                case MP_INT16:
                    return ReadInt16(reader);
                case MP_INT32:
                    return ReadInt32(reader);
                case MP_INT64:
                    return ReadInt64(reader);
                case MP_ARRAY16:
                    return UnpackList(ReadInt16(reader) & MAX_16BIT, reader, options);
                case MP_ARRAY32:
                    return UnpackList(ReadInt32(reader), reader, options);
                case MP_MAP16:
                    return UnpackMap(ReadInt16(reader) & MAX_16BIT, reader, options);
                case MP_MAP32:
                    return UnpackMap(ReadInt32(reader), reader, options);
                case MP_RAW8:
                    return UnpackRaw(reader.ReadByte() & MAX_8BIT, reader, options);
                case MP_RAW16:
                    return UnpackRaw(ReadInt16(reader) & MAX_16BIT, reader, options);
                case MP_RAW32:
                    return UnpackRaw(ReadInt32(reader), reader, options);
            }

            if ((value & (1 << 7)) == 0)
                return value;

            if ((value & MP_NEGATIVE_FIXNUM) == MP_NEGATIVE_FIXNUM_INT)
                return (sbyte) value;

            if (value >= MP_NEGATIVE_FIXNUM_INT && value <= MP_NEGATIVE_FIXNUM_INT + MAX_5BIT)
            {
                return value;
            }

            if (value >= MP_FIXARRAY_INT && value <= MP_FIXARRAY_INT + MAX_4BIT)
            {
                return UnpackList(value - MP_FIXARRAY_INT, reader, options);
            }

            if (value >= MP_FIXMAP_INT && value <= MP_FIXMAP_INT + MAX_4BIT)
            {
                return UnpackMap(value - MP_FIXMAP_INT, reader, options);
            }

            if (value >= MP_FIXRAW_INT && value <= MP_FIXRAW_INT + MAX_5BIT)
            {
                return UnpackRaw(value - MP_FIXRAW_INT, reader, options);
            }

            if (value <= MAX_7BIT)
            {//MP_FIXNUM - the value is value as an int
                return value;
            }

            throw new ArgumentException("Input contains invalid type value " + (byte)value);
        }

        private static long ReadInt64(BinaryReader reader)
        {
            return BitConverter.ToInt64(ReverseArray(reader.ReadBytes(8)), 0);
        }

        private static int ReadInt32(BinaryReader reader)
        {
            return BitConverter.ToInt32(ReverseArray(reader.ReadBytes(4)), 0);
        }

        private static short ReadInt16(BinaryReader reader)
        {
            return BitConverter.ToInt16(ReverseArray(reader.ReadBytes(2)), 0);
        }

        private static ulong ReadUInt64(BinaryReader reader)
        {
            return BitConverter.ToUInt64(ReverseArray(reader.ReadBytes(8)), 0);
        }

        private static uint ReadUInt32(BinaryReader reader)
        {
            return BitConverter.ToUInt32(ReverseArray(reader.ReadBytes(4)), 0);
        }

        private static ushort ReadUInt16(BinaryReader reader)
        {
            return BitConverter.ToUInt16(ReverseArray(reader.ReadBytes(2)), 0);
        }

        private static double ReadDouble(BinaryReader reader)
        {
            return BitConverter.ToDouble(ReverseArray(reader.ReadBytes(8)), 0);
        }

        private static float ReadSingle(BinaryReader reader)
        {
            return BitConverter.ToSingle(ReverseArray(reader.ReadBytes(4)), 0);
        }

        private static object UnpackRaw(int size, BinaryReader reader, int options)
        {
            if (size < 0)
            {
                throw new ArgumentException("byte[] to unpack too large (more than 2^31 elements)!");
            }

            var data = new byte[size];

            reader.Read(data, 0, size);

            if ((options & OPTION_UNPACK_RAW_AS_BYTE_BUFFER) != 0)
            {
                return new MemoryStream(data);
            }

            if ((options & OPTION_UNPACK_RAW_AS_STRING) != 0)
            {
                return System.Text.Encoding.UTF8.GetString(data);
            }

            return data;
        }

        private static IDictionary UnpackMap(int size, BinaryReader reader, int options)
        {
            if (size < 0)
            {
                throw new ArgumentException("Map to unpack too large (more than 2^31 elements)!");
            }

            var ret = new Dictionary<object, object>(size);
            for (var i = 0; i < size; ++i)
            {
                var key = Unpack(reader, options);
                var value = Unpack(reader, options);
                ret.Add(key, value);
            }

            return ret;
        }

        private static IList UnpackList(int size, BinaryReader reader, int options)
        {
            if (size < 0)
            {
                throw new ArgumentException("Array to unpack too large (more than 2^31 elements)!");
            }
            var ret = new object[size];

            for (var i = 0; i < size; ++i)
            {
                ret[i] = Unpack(reader, options);
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
                return (sbyte)item;
            if (item is byte)
                return (byte)item;
            if (item is short)
                return (short)item;
            if (item is ushort)
                return (ushort)item;
            if (item is int)
                return (int)item;
            if (item is uint)
                return (uint)item;
            if (item is long)
                return (long)item;
            if (item is ulong)
                return (long)(ulong)item;

            throw new ArgumentException("Item can't be converted to long :" + item);
        }



    }
}