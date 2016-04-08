using System;
using System.IO;
using TarantoolDnx.MsgPack.Interfaces;

namespace TarantoolDnx.MsgPack
{
    internal sealed class BytesWriter : IBytesWriter
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
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(double item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(ushort item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(short item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(uint item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(int item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(ulong item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        private static byte[] ToBigEndianBytes(long item)
        {
            return ByteUtils.ReverseArrayIfNeeded(BitConverter.GetBytes(item));
        }

        #endregion
    }
}