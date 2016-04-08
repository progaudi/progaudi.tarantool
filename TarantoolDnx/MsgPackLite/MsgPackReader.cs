using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MsgPackLite.Interfaces;

namespace MsgPackLite
{
    public class MsgPackReader : IMsgPackReader
    {
        private readonly IBytesReader reader;

        public MsgPackReader(Stream bytesReader)
        {
            reader = new BytesReader(bytesReader);
        }

        public string ReadString()
        {
            var value = reader.ReadByte();
            switch (value)
            {
                case MsgPackConstants.MpNull:
                    return null;
                case MsgPackConstants.MpStr8:
                    return ReadString(reader.ReadByte() & MsgPackConstants.Max8Bit);
                case MsgPackConstants.MpStr16:
                    return ReadString(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpStr32:
                    return ReadString(reader.ReadInt32());
            }
            if (value >= MsgPackConstants.MpFixstr && value <= MsgPackConstants.MpFixstr + MsgPackConstants.Max5Bit)
            {
                return ReadString(value - MsgPackConstants.MpFixstr);
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public double ReadDouble()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpDouble)
            {
                return reader.ReadDouble();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public float ReadFloat()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpFloat)
            {
                return reader.ReadFloat();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public bool ReadBool()
        {
            var value = reader.ReadByte();

            switch (value)
            {
                case MsgPackConstants.MpFalse:
                    return false;
                case MsgPackConstants.MpTrue:
                    return true;
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public byte ReadByte()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpUint8)
            {
                return reader.ReadByte();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public sbyte ReadSByte()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpInt8)
            {
                return reader.ReadSByte();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public short ReadShort()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpInt16)
            {
                return reader.ReadInt16();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public ushort ReadUShort()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpUint16)
            {
                return reader.ReadUInt16();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public int ReadInt()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpInt32)
            {
                return reader.ReadInt32();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public uint ReadUInt()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpUint32)
            {
                return reader.ReadUInt32();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public long ReadLong()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpInt64)
            {
                return reader.ReadInt64();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public ulong ReadULong()
        {
            var value = reader.ReadByte();
            if (value == MsgPackConstants.MpUint64)
            {
                return reader.ReadUInt64();
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public byte[] ReadBinary()
        {
            var value = reader.ReadByte();
            switch (value)
            {
                case MsgPackConstants.MpNull:
                    return null;
                case MsgPackConstants.MpBit8:
                    return ReadBinary(reader.ReadByte() & MsgPackConstants.Max8Bit);
                case MsgPackConstants.MpBit16:
                    return ReadBinary(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpBit32:
                    return ReadBinary(reader.ReadInt32());
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public T[] ReadArray<T>()
        {
            var value = reader.ReadByte();
            switch (value)
            {
                case MsgPackConstants.MpNull:
                    return null;
                case MsgPackConstants.MpArray16:
                    return ReadArray<T>(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpArray32:
                    return ReadArray<T>(reader.ReadInt32());
            }

            if (value >= MsgPackConstants.MpFixarray && value <= MsgPackConstants.MpFixarray + MsgPackConstants.Max4Bit)
            {
                return ReadArray<T>(value - MsgPackConstants.MpFixarray);
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        public IDictionary<TK, TV> ReadMap<TK, TV>()
        {
            var value = reader.ReadByte();
            switch (value)
            {
                case MsgPackConstants.MpNull:
                    return null;
                case MsgPackConstants.MpMap16:
                    return ReadMap<TK, TV>(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpMap32:
                    return ReadMap<TK, TV>(reader.ReadInt32());
            }

            if (value >= MsgPackConstants.MpFixmap && value <= MsgPackConstants.MpFixmap + MsgPackConstants.Max4Bit)
            {
                return ReadMap<TK, TV>(value - MsgPackConstants.MpFixmap);
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }

        private string ReadString(int size)
        {
            if (size < 0)
            {
                throw new ArgumentException("String to unpack too large (more than 2^31 elements)!");
            }

            var data = reader.ReadBytes(size);

            return Encoding.UTF8.GetString(data);
        }

        private byte[] ReadBinary(int size)
        {
            if (size < 0)
            {
                throw new ArgumentException("byte[] to unpack too large (more than 2^31 elements)!");
            }

            var data = reader.ReadBytes(size);

            return data;
        }

        private T[] ReadArray<T>(int size)
        {
            if (size < 0)
            {
                throw new ArgumentException("Array to unpack too large (more than 2^31 elements)!");
            }
            var ret = new T[size];

            for (var i = 0; i < size; ++i)
            {
                ret[i] = (T)ReadObject();
            }

            return ret;
        }

        private IDictionary<TK, TV> ReadMap<TK, TV>(int size)
        {
            if (size < 0)
            {
                throw new ArgumentException("Map to unpack too large (more than 2^31 elements)!");
            }

            var ret = new Dictionary<TK, TV>(size);
            for (var i = 0; i < size; ++i)
            {
                var key = (TK)ReadObject();
                var value = (TV)ReadObject();
                ret.Add(key, value);
            }

            return ret;
        }

        private object ReadObject()
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
                    return reader.ReadFloat();
                case MsgPackConstants.MpDouble:
                    return reader.ReadDouble();
                case MsgPackConstants.MpUint8:
                    return reader.ReadByte();
                case MsgPackConstants.MpUint16:
                    return reader.ReadUInt16();
                case MsgPackConstants.MpUint32:
                    return reader.ReadUInt32();
                case MsgPackConstants.MpUint64:
                    return reader.ReadUInt64();
                case MsgPackConstants.MpInt8:
                    return reader.ReadSByte();
                case MsgPackConstants.MpInt16:
                    return reader.ReadInt16();
                case MsgPackConstants.MpInt32:
                    return reader.ReadInt32();
                case MsgPackConstants.MpInt64:
                    return reader.ReadInt64();
                case MsgPackConstants.MpArray16:
                    return ReadArray<object>(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpArray32:
                    return ReadArray<object>(reader.ReadInt32());
                case MsgPackConstants.MpMap16:
                    return ReadMap<object, object>(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpMap32:
                    return ReadMap<object, object>(reader.ReadInt32());
                case MsgPackConstants.MpStr8:
                    return ReadString(reader.ReadByte() & MsgPackConstants.Max8Bit);
                case MsgPackConstants.MpStr16:
                    return ReadString(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpStr32:
                    return ReadString(reader.ReadInt32());
                case MsgPackConstants.MpBit8:
                    return ReadBinary(reader.ReadByte() & MsgPackConstants.Max8Bit);
                case MsgPackConstants.MpBit16:
                    return ReadBinary(reader.ReadInt16() & MsgPackConstants.Max16Bit);
                case MsgPackConstants.MpBit32:
                    return ReadBinary(reader.ReadInt32());
            }

            if ((value & MsgPackConstants.MpNegativeFixnum) == MsgPackConstants.MpNegativeFixnum)
                return (sbyte)value;

            if (value >= MsgPackConstants.MpNegativeFixnum && value <= MsgPackConstants.MpNegativeFixnum + MsgPackConstants.Max5Bit)
            {
                return value;
            }

            if (value >= MsgPackConstants.MpFixarray && value <= MsgPackConstants.MpFixarray + MsgPackConstants.Max4Bit)
            {
                return ReadArray<object>(value - MsgPackConstants.MpFixarray);
            }

            if (value >= MsgPackConstants.MpFixmap && value <= MsgPackConstants.MpFixmap + MsgPackConstants.Max4Bit)
            {
                return ReadMap<object, object>(value - MsgPackConstants.MpFixmap);
            }

            if (value >= MsgPackConstants.MpFixstr && value <= MsgPackConstants.MpFixstr + MsgPackConstants.Max5Bit)
            {
                return ReadString(value - MsgPackConstants.MpFixstr);
            }

            if (value <= MsgPackConstants.Max7Bit)
            {
                //MP_FIXNUM - the value is value as an int
                return value;
            }

            throw new ArgumentException("Input contains invalid type value " + value);
        }
    }
}