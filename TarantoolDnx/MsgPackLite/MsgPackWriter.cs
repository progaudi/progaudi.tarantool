using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using MsgPackLite.Interfaces;

namespace MsgPackLite
{
    public class MsgPackWriter : IMsgPackWriter
    {
        private readonly IBytesWriter _writer;

        public MsgPackWriter(Stream innerStream)
        {
            _writer = new BytesWriter(innerStream);
        }

        public void Write(string item)
        {
            if (item == null)
            {
                WriteNull();
            }
            else
            {
                var data = Encoding.UTF8.GetBytes(item);

                if (data.Length <= MsgPackConstants.Max5Bit)
                {
                    _writer.Write((byte)(data.Length | MsgPackConstants.MpFixstr));
                }
                else if (data.Length <= MsgPackConstants.Max8Bit)
                {
                    _writer.Write(MsgPackConstants.MpStr8);
                    _writer.Write((byte)data.Length);
                }
                else if (data.Length <= MsgPackConstants.Max16Bit)
                {
                    _writer.Write(MsgPackConstants.MpStr16);
                    _writer.WriteBigEndianBytes((ushort)data.Length);
                }
                else
                {
                    _writer.Write(MsgPackConstants.MpStr32);
                    _writer.WriteBigEndianBytes((uint)data.Length);
                }
                _writer.Write(data);
            }
        }

        public void Write(double item)
        {
            _writer.Write(MsgPackConstants.MpDouble);
            _writer.WriteBigEndianBytes(item);
        }

        public void Write(float item)
        {
            _writer.Write(MsgPackConstants.MpFloat);
            _writer.WriteBigEndianBytes(item);
        }

        public void Write(bool item)
        {
            _writer.Write(item ? MsgPackConstants.MpTrue : MsgPackConstants.MpFalse);
        }

        public void Write(byte item)
        {
            if (item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write(item);
            }
            else
            {
                _writer.Write(MsgPackConstants.MpUint8);
                _writer.Write(item);
            }
        }

        public void Write(sbyte item)
        {
            var byteValue = (byte)item;

            if (item >= 0 && item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else if (item < 0 && byteValue >= MsgPackConstants.MpNegativeFixnum && byteValue <= MsgPackConstants.Max8Bit)
            {
                _writer.Write((byte)(byteValue | MsgPackConstants.MpNegativeFixnum));
            }
            else
            {
                _writer.Write(MsgPackConstants.MpInt8);
                _writer.Write(item);
            }
        }

        public void Write(short item)
        {
            var byteValue = (byte)item;

            if (item >= 0 && item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else if (item < 0 && byteValue >= MsgPackConstants.MpNegativeFixnum && byteValue <= MsgPackConstants.Max8Bit)
            {
                _writer.Write((byte)(byteValue | MsgPackConstants.MpNegativeFixnum));
            }
            else
            {
                _writer.Write(MsgPackConstants.MpInt16);
                _writer.WriteBigEndianBytes(item);
            }
        }

        public void Write(ushort item)
        {
            if (item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else
            {
                _writer.Write(MsgPackConstants.MpUint16);
                _writer.WriteBigEndianBytes(item);
            }
        }

        public void Write(int item)
        {
            var byteValue = (byte)item;

            if (item >= 0 && item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else if (item < 0 && byteValue >= MsgPackConstants.MpNegativeFixnum && byteValue <= MsgPackConstants.Max8Bit)
            {
                _writer.Write((byte)(byteValue | MsgPackConstants.MpNegativeFixnum));
            }
            else
            {
                _writer.Write(MsgPackConstants.MpInt32);
                _writer.WriteBigEndianBytes(item);
            }
        }

        public void Write(uint item)
        {
            if (item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else
            {
                _writer.Write(MsgPackConstants.MpUint32);
                _writer.WriteBigEndianBytes(item);
            }
        }

        public void Write(long item)
        {
            var byteValue = (byte)item;

            if (item >= 0 && item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else if (item < 0 && byteValue >= MsgPackConstants.MpNegativeFixnum && byteValue <= MsgPackConstants.Max8Bit)
            {
                _writer.Write((byte)(byteValue | MsgPackConstants.MpNegativeFixnum));
            }
            else
            {
                _writer.Write(MsgPackConstants.MpInt64);
                _writer.WriteBigEndianBytes(item);
            }
        }

        public void Write(ulong item)
        {
            if (item <= MsgPackConstants.Max7Bit)
            {
                _writer.Write((byte)item);
            }
            else
            {
                _writer.Write(MsgPackConstants.MpUint64);
                _writer.WriteBigEndianBytes(item);
            }
        }

        public void Write(byte[] data)
        {
            if (data == null)
            {
                WriteNull();
            }
            else
            {
                if (data.Length <= MsgPackConstants.Max8Bit)
                {
                    _writer.Write(MsgPackConstants.MpBit8);
                    _writer.Write((byte)data.Length);
                }
                else if (data.Length <= MsgPackConstants.Max16Bit)
                {
                    _writer.Write(MsgPackConstants.MpBit16);
                    _writer.WriteBigEndianBytes((ushort)data.Length);
                }
                else
                {
                    _writer.Write(MsgPackConstants.MpBit32);
                    _writer.WriteBigEndianBytes((uint)data.Length);
                }

                _writer.Write(data);
            }
        }

        public void Write(IList list)
        {
            if (list == null)
            {
                WriteNull();
            }
            else
            {
                var length = list.Count;

                if (length <= MsgPackConstants.Max4Bit)
                {
                    _writer.Write((byte)(length | MsgPackConstants.MpFixarray));
                }
                else if (length <= MsgPackConstants.Max16Bit)
                {
                    _writer.Write(MsgPackConstants.MpArray16);
                    _writer.WriteBigEndianBytes((ushort)length);
                }
                else
                {
                    _writer.Write(MsgPackConstants.MpArray32);
                    _writer.WriteBigEndianBytes((uint)length);
                }
                foreach (var element in list)
                {
                    Write(element);
                }
            }
        }

        public void Write<TK, TV>(IDictionary<TK, TV> map)
        {
            if (map == null)
            {
                WriteNull();
            }
            else
            {
                if (map.Count <= MsgPackConstants.Max4Bit)
                {
                    _writer.Write((byte)(map.Count | MsgPackConstants.MpFixmap));
                }
                else if (map.Count <= MsgPackConstants.Max16Bit)
                {
                    _writer.Write(MsgPackConstants.MpMap16);
                    _writer.WriteBigEndianBytes((ushort)map.Count);
                }
                else
                {
                    _writer.Write(MsgPackConstants.MpMap32);
                    _writer.WriteBigEndianBytes((uint)map.Count);
                }
                foreach (KeyValuePair<TK, TV> kvp in map)
                {
                    Write(kvp.Key);
                    Write(kvp.Value);
                }
            }
        }

        private void Write(object item)
        {
            if (item == null)
            {
                WriteNull();
            }
            else if (item is bool)
            {
                Write((bool)item);
            }
            else if (item is float)
            {
                Write((float)item);
            }
            else if (item is double)
            {
                Write((double)item);
            }
            else if (item is byte)
            {
                Write((byte)item);
            }
            else if (item is sbyte)
            {
                Write((sbyte)item);
            }
            else if (item is short)
            {
                Write((short)item);
            }
            else if (item is ushort)
            {
                Write((ushort)item);
            }
            else if (item is int)
            {
                Write((int)item);
            }
            else if (item is uint)
            {
                Write((uint)item);
            }
            else if (item is long)
            {
                Write((long)item);
            }
            else if (item is ulong)
            {
                Write((ulong)item);
            }
            else if (item is string)
            {
                Write((string)item);
            }
            else if (item is byte[])
            {
                Write((byte[])item);
            }
            else if (item is IList)
            {
                Write((IList)item);
            }
            else if (item is IDictionary)
            {
                Write((IDictionary)item);
            }
            else
            {
                throw new ArgumentException("Cannot msgWrite object of type " + item.GetType().FullName);
            }
        }

        private void Write(ICollection dict)
        {
            var castedDict = CastDict(dict);
            Write(castedDict);
        }

        private static IDictionary<object, object> CastDict(ICollection dictionary)
        {
            var result = new Dictionary<object, object>(dictionary.Count);
            foreach (DictionaryEntry entry in dictionary)
            {
                result.Add(entry.Key, entry.Value);
            }
            return result;
        }

        private void WriteNull()
        {
            _writer.Write(MsgPackConstants.MpNull);
        }
    }
}