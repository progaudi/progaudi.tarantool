using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MsgPackLite
{
    public class MsgPackLite
    {
        #region Public Methods

        public static object Unpack(byte[] bytesArray)
        {
            using (var stream = new MemoryStream(bytesArray))
            {
                using (var reader = new BinaryReader(stream))
                {
                    return Unpack(reader);
                }
            }
        }

        public static byte[] Pack(object item)
        {
            using (var stream = new MemoryStream())
            {
                using (var writer = new BinaryWriter(stream))
                {
                    Pack(item, writer);
                    return stream.ToArray();
                }
            }
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
            writer.Write(MsgPackConstants.MpDouble);
            writer.Write(ToBigEndianBytes(item));
        }

        private static void PackFloat(float item, BinaryWriter writer)
        {
            writer.Write(MsgPackConstants.MpFloat);
            writer.Write(ToBigEndianBytes(item));
        }

        private static void PackBoolean(bool item, BinaryWriter writer)
        {
            writer.Write(item ? MsgPackConstants.MpTrue : MsgPackConstants.MpFalse);
        }

        private static void PackNull(BinaryWriter writer)
        {
            writer.Write(MsgPackConstants.MpNull);
        }

        private static void PackString(string item, BinaryWriter writer)
        {
            var data = Encoding.UTF8.GetBytes(item);

            if (data.Length <= MsgPackConstants.Max5Bit)
            {
                writer.Write((byte)(data.Length | MsgPackConstants.MpFixstr));
            }
            else if (data.Length <= MsgPackConstants.Max8Bit)
            {
                writer.Write(MsgPackConstants.MpStr8);
                writer.Write((byte)data.Length);
            }
            else if (data.Length <= MsgPackConstants.Max16Bit)
            {
                writer.Write(MsgPackConstants.MpStr16);
                writer.Write(ToBigEndianBytes((ushort)data.Length));
            }
            else
            {
                writer.Write(MsgPackConstants.MpStr32);
                writer.Write(ToBigEndianBytes((uint)data.Length));
            }
            writer.Write(data);
        }

        private static void PackByteArray(byte[] data, BinaryWriter writer)
        {
            if (data.Length <= MsgPackConstants.Max8Bit)
            {
                writer.Write(MsgPackConstants.MpBit8);
                writer.Write((byte)data.Length);
            }
            else if (data.Length <= MsgPackConstants.Max16Bit)
            {
                writer.Write(MsgPackConstants.MpBit16);
                writer.Write(ToBigEndianBytes((ushort)data.Length));
            }
            else
            {
                writer.Write(MsgPackConstants.MpBit32);
                writer.Write(ToBigEndianBytes((uint)data.Length));
            }

            writer.Write(data);
        }

        private static void PackMap(IDictionary item, BinaryWriter writer)
        {
            var map = (IDictionary<object, object>)item;
            if (map.Count <= MsgPackConstants.Max4Bit)
            {
                writer.Write((byte)(map.Count | MsgPackConstants.MpFixmap));
            }
            else if (map.Count <= MsgPackConstants.Max16Bit)
            {
                writer.Write(MsgPackConstants.MpMap16);
                writer.Write(ToBigEndianBytes((ushort)map.Count));
            }
            else
            {
                writer.Write(MsgPackConstants.MpMap32);
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

            if (length <= MsgPackConstants.Max4Bit)
            {
                writer.Write((byte)(length | MsgPackConstants.MpFixarray));
            }
            else if (length <= MsgPackConstants.Max16Bit)
            {
                writer.Write(MsgPackConstants.MpArray16);
                writer.Write(ToBigEndianBytes((ushort)length));
            }
            else
            {
                writer.Write(MsgPackConstants.MpArray32);
                writer.Write(ToBigEndianBytes((uint)length));
            }
            foreach (var element in list)
            {
                Pack(element, writer);
            }
        }

        private static void PackIntegerNumber(object item, BinaryWriter outputWriter)
        {
            var longValue = CastToLong(item);
            var byteValue = (byte)longValue;
            if (item is ulong && (ulong)item> MsgPackConstants.Max7Bit)
            {
                outputWriter.Write(MsgPackConstants.MpUint64);
                outputWriter.Write(ToBigEndianBytes((ulong)item));
            }
            else if (longValue >= 0 && longValue <= MsgPackConstants.Max7Bit)
            {
                outputWriter.Write((byte) longValue);
            }
            else if (longValue < 0 && byteValue>= MsgPackConstants.MpNegativeFixnum && byteValue <= MsgPackConstants.Max8Bit)
            {
                outputWriter.Write((byte) (byteValue | MsgPackConstants.MpNegativeFixnum));
            }
            else if(item is byte)
            {
                outputWriter.Write(MsgPackConstants.MpUint8);
                outputWriter.Write((byte) item);
            }
            else if (item is ushort)
            {
                outputWriter.Write(MsgPackConstants.MpUint16);
                outputWriter.Write(ToBigEndianBytes((ushort)item));
            }
            else if (item is uint)
            {
                outputWriter.Write(MsgPackConstants.MpUint32);
                outputWriter.Write(ToBigEndianBytes((uint)item));
            }
            else if (item is sbyte)
            {
                outputWriter.Write(MsgPackConstants.MpInt8);
                outputWriter.Write((sbyte)item);
            }
            else if (item is short)
            {
                outputWriter.Write(MsgPackConstants.MpInt16);
                outputWriter.Write(ToBigEndianBytes((short)item));
            }
            else if (item is int)
            {
                outputWriter.Write(MsgPackConstants.MpInt32);
                outputWriter.Write(ToBigEndianBytes((int)item));
            }
            else if (item is long)
            {
                outputWriter.Write(MsgPackConstants.MpInt64);
                outputWriter.Write(ToBigEndianBytes((long)item));
            }
        }

        #endregion

        #region Unpacking

        private static object Unpack(BinaryReader reader)
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case MsgPackConstants.MpNull:
                    return null;
                case MsgPackConstants.MpFalse:
                    return false;
                case MsgPackConstants.MpTrue:
                    return true;
                case MsgPackConstants.MpFloat:
                    return ReadSingle(reader);
                case MsgPackConstants.MpDouble:
                    return ReadDouble(reader);
                case MsgPackConstants.MpUint8:
                    return reader.ReadByte();
                case MsgPackConstants.MpUint16:
                    return ReadUInt16(reader);
                case MsgPackConstants.MpUint32:
                    return ReadUInt32(reader);
                case MsgPackConstants.MpUint64:
                    return ReadUInt64(reader);
                case MsgPackConstants.MpInt8:
                    return reader.ReadSByte();
                case MsgPackConstants.MpInt16:
                    return ReadInt16(reader);
                case MsgPackConstants.MpInt32:
                    return ReadInt32(reader);
                case MsgPackConstants.MpInt64:
                    return ReadInt64(reader);
                case MsgPackConstants.MpArray16:
                    return UnpackList(ReadInt16(reader) & MsgPackConstants.Max16Bit, reader);
                case MsgPackConstants.MpArray32:
                    return UnpackList(ReadInt32(reader), reader);
                case MsgPackConstants.MpMap16:
                    return UnpackMap(ReadInt16(reader) & MsgPackConstants.Max16Bit, reader);
                case MsgPackConstants.MpMap32:
                    return UnpackMap(ReadInt32(reader), reader);
                case MsgPackConstants.MpStr8:
                    return UnpackString(reader.ReadByte() & MsgPackConstants.Max8Bit, reader);
                case MsgPackConstants.MpStr16:
                    return UnpackString(ReadInt16(reader) & MsgPackConstants.Max16Bit, reader);
                case MsgPackConstants.MpStr32:
                    return UnpackString(ReadInt32(reader), reader);
                case MsgPackConstants.MpBit8:
                    return UnpackBin(reader.ReadByte() & MsgPackConstants.Max8Bit, reader);
                case MsgPackConstants.MpBit16:
                    return UnpackBin(ReadInt16(reader) & MsgPackConstants.Max16Bit, reader);
                case MsgPackConstants.MpBit32:
                    return UnpackBin(ReadInt32(reader), reader);
            }

            if ((value & MsgPackConstants.MpNegativeFixnum) == MsgPackConstants.MpNegativeFixnum)
                return (sbyte) value;

            if (value >= MsgPackConstants.MpNegativeFixnum && value <= MsgPackConstants.MpNegativeFixnum + MsgPackConstants.Max5Bit)
            {
                return value;
            }

            if (value >= MsgPackConstants.MpFixarray && value <= MsgPackConstants.MpFixarray + MsgPackConstants.Max4Bit)
            {
                return UnpackList(value - MsgPackConstants.MpFixarray, reader);
            }

            if (value >= MsgPackConstants.MpFixmap && value <= MsgPackConstants.MpFixmap + MsgPackConstants.Max4Bit)
            {
                return UnpackMap(value - MsgPackConstants.MpFixmap, reader);
            }

            if (value >= MsgPackConstants.MpFixstr && value <= MsgPackConstants.MpFixstr + MsgPackConstants.Max5Bit)
            {
                return UnpackString(value - MsgPackConstants.MpFixstr, reader);
            }

            if (value <= MsgPackConstants.Max7Bit)
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