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

        public static void Pack(object item, Stream os)
        {
            var outputWriter = new BinaryWriter(os);

            if (item == null)
            {
                outputWriter.Write(MP_NULL);
            }
            else if (item is bool)
            {
                outputWriter.Write((bool)item ? MP_TRUE : MP_FALSE);
            }
            else if (item is float)
            {
                outputWriter.Write(MP_FLOAT);
                outputWriter.Write((float)item);
            }
            else if (item is double)
            {
                outputWriter.Write(MP_DOUBLE);
                outputWriter.Write((double)item);
            }
            else if (IsIntegerNumber(item))
            {
                var value = (long)item;
                if (value >= 0)
                {
                    if (value <= MAX_7BIT)
                    {
                        outputWriter.Write((int)value | MP_FIXNUM);
                    }
                    else if (value <= MAX_8BIT)
                    {
                        outputWriter.Write(MP_UINT8);
                        outputWriter.Write((int)value);
                    }
                    else if (value <= MAX_16BIT)
                    {
                        outputWriter.Write(MP_UINT16);
                        outputWriter.Write((short)value);
                    }
                    else if (value <= MAX_32BIT)
                    {
                        outputWriter.Write(MP_UINT32);
                        outputWriter.Write((int)value);
                    }
                    else
                    {
                        outputWriter.Write(MP_UINT64);
                        outputWriter.Write(value);
                    }
                }
                else
                {
                    if (value >= -(MAX_5BIT + 1))
                    {
                        outputWriter.Write((int)(value & 0xff));
                    }
                    else if (value >= -(MAX_7BIT + 1))
                    {
                        outputWriter.Write(MP_INT8);
                        outputWriter.Write((int)value);
                    }
                    else if (value >= -(MAX_15BIT + 1))
                    {
                        outputWriter.Write(MP_INT16);
                        outputWriter.Write((short)value);
                    }
                    else if (value >= -MAX_31BIT)
                    {
                        outputWriter.Write(MP_INT32);
                        outputWriter.Write((int)value);
                    }
                    else
                    {
                        outputWriter.Write(MP_INT64);
                        outputWriter.Write(value);
                    }
                }
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

                if (data.Length <= MAX_5BIT)
                {
                    outputWriter.Write(data.Length | MP_FIXRAW);
                }
                else if (data.Length <= MAX_8BIT)
                {
                    outputWriter.Write(MP_RAW8);
                    outputWriter.Write((byte)data.Length);
                }
                else if (data.Length <= MAX_16BIT)
                {
                    outputWriter.Write(MP_RAW16);
                    outputWriter.Write((short)data.Length);
                }
                else
                {
                    outputWriter.Write(MP_RAW32);
                    outputWriter.Write(data.Length);
                }
                outputWriter.Write(data);
            }
            else
            {
                var list1 = item as IList;
                if (list1 != null)
                {
                    var list = list1;
                    var length = list.Count;

                    if (length <= MAX_4BIT)
                    {
                        outputWriter.Write(length | MP_FIXARRAY);
                    }
                    else if (length <= MAX_16BIT)
                    {
                        outputWriter.Write(MP_ARRAY16);
                        outputWriter.Write((short)length);
                    }
                    else
                    {
                        outputWriter.Write(MP_ARRAY32);
                        outputWriter.Write(length);
                    }
                    foreach (var element in list)
                    {
                        Pack(element, outputWriter.BaseStream);
                    }
                }
                else if (item is IDictionary)
                {
                    var map = (Dictionary<Object, Object>)item;
                    if (map.Count <= MAX_4BIT)
                    {
                        outputWriter.Write(map.Count | MP_FIXMAP);
                    }
                    else if (map.Count <= MAX_16BIT)
                    {
                        outputWriter.Write(MP_MAP16);
                        outputWriter.Write((short)map.Count);
                    }
                    else
                    {
                        outputWriter.Write(MP_MAP32);
                        outputWriter.Write(map.Count);
                    }
                    foreach (var kvp in map)
                    {
                        Pack(kvp.Key, outputWriter.BaseStream);
                        Pack(kvp.Value, outputWriter.BaseStream);
                    }
                }
                else
                {
                    throw new ArgumentException("Cannot msgpack object of type " + item.GetType().FullName);
                }
            }
        }

        public static object Unpack(Stream stream, int options)
        {
            var inputStream = new BinaryReader(stream);
            int value = inputStream.ReadByte();
            if (value < 0)
            {
                throw new ArgumentException("No more input available when expecting a value");
            }

            switch ((byte)value)
            {
                case MP_NULL:
                    return null;
                case MP_FALSE:
                    return false;
                case MP_TRUE:
                    return true;
                case MP_FLOAT:
                    return inputStream.ReadSingle();
                case MP_DOUBLE:
                    return inputStream.ReadDouble();
                case MP_UINT8:
                    return inputStream.ReadByte();
                case MP_UINT16:
                    return inputStream.ReadUInt16();
                case MP_UINT32:
                    return inputStream.ReadUInt32();
                case MP_UINT64:
                    return inputStream.ReadUInt64();
                case MP_INT8:
                    return inputStream.ReadByte();
                case MP_INT16:
                    return inputStream.ReadInt16();
                case MP_INT32:
                    return inputStream.ReadInt32();
                case MP_INT64:
                    return inputStream.ReadInt64();
                case MP_ARRAY16:
                    return UnpackList(inputStream.ReadInt16() & MAX_16BIT, inputStream.BaseStream, options);
                case MP_ARRAY32:
                    return UnpackList(inputStream.ReadInt32(), inputStream.BaseStream, options);
                case MP_MAP16:
                    return UnpackMap(inputStream.ReadInt16() & MAX_16BIT, inputStream.BaseStream, options);
                case MP_MAP32:
                    return UnpackMap(inputStream.ReadInt32(), inputStream.BaseStream, options);
                case MP_RAW8:
                    return UnpackRaw(inputStream.ReadByte() & MAX_8BIT, inputStream.BaseStream, options);
                case MP_RAW16:
                    return UnpackRaw(inputStream.ReadInt16() & MAX_16BIT, inputStream.BaseStream, options);
                case MP_RAW32:
                    return UnpackRaw(inputStream.ReadInt32(), inputStream.BaseStream, options);
            }

            if (value >= MP_NEGATIVE_FIXNUM_INT && value <= MP_NEGATIVE_FIXNUM_INT + MAX_5BIT)
            {
                return (byte)value;
            }

            if (value >= MP_FIXARRAY_INT && value <= MP_FIXARRAY_INT + MAX_4BIT)
            {
                return UnpackList(value - MP_FIXARRAY_INT, inputStream.BaseStream, options);
            }

            if (value >= MP_FIXMAP_INT && value <= MP_FIXMAP_INT + MAX_4BIT)
            {
                return UnpackMap(value - MP_FIXMAP_INT, inputStream.BaseStream, options);
            }

            if (value >= MP_FIXRAW_INT && value <= MP_FIXRAW_INT + MAX_5BIT)
            {
                return UnpackRaw(value - MP_FIXRAW_INT, inputStream.BaseStream, options);
            }

            if (value <= MAX_7BIT)
            {//MP_FIXNUM - the value is value as an int
                return value;
            }

            throw new ArgumentException("Input contains invalid type value " + (byte)value);
        }

        private static object UnpackRaw(int size, Stream inputStream, int options)
        {
            if (size < 0)
            {
                throw new ArgumentException("byte[] to unpack too large (more than 2^31 elements)!");
            }

            var data = new byte[size];

            inputStream.Read(data, 0, size);

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

        private static IDictionary UnpackMap(int size, Stream inputStream, int options)
        {
            if (size < 0)
            {
                throw new ArgumentException("Map to unpack too large (more than 2^31 elements)!");
            }

            var ret = new Dictionary<object, object>(size);
            for (var i = 0; i < size; ++i)
            {
                var key = Unpack(inputStream, options);
                var value = Unpack(inputStream, options);
                ret.Add(key, value);
            }

            return ret;
        }

        private static IList UnpackList(int size, Stream inputStream, int options)
        {
            if (size < 0)
            {
                throw new ArgumentException("Array to unpack too large (more than 2^31 elements)!");
            }
            var ret = new ArrayList(size);

            for (var i = 0; i < size; ++i)
            {
                ret.Add(Unpack(inputStream, options));
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
    }
}