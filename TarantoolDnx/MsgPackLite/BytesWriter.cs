using System;
using System.IO;

namespace MsgPackLite
{
    public class BytesWriter : IBytesWriter
    {
        private readonly Stream _innerStream;

        public BytesWriter(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public void Write(byte data)
        {
            _innerStream.WriteByte(data);
        }

        public void Write(sbyte data)
        {
            throw new NotImplementedException();
        }

        public void Write(byte[] data)
        {
            _innerStream.Write(data, 0, data.Length);
        }

        public void WriteBigEndianBytes(double data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(float data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(short data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(ushort data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(int data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(uint data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(long data)
        {
            Write(ToBigEndianBytes(data));
        }

        public void WriteBigEndianBytes(ulong data)
        {
            Write(ToBigEndianBytes(data));
        }
        
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
    }
}