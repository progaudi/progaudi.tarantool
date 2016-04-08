using System;
using System.IO;
using MsgPackLite.Interfaces;

namespace MsgPackLite
{
    internal sealed class BytesReader : IBytesReader
    {
        private readonly Stream _innerStream;

        public BytesReader(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public sbyte ReadSByte()
        {
            return (sbyte)_innerStream.ReadByte();
        }

        public byte ReadByte()
        {
            return (byte)_innerStream.ReadByte();
        }

        public short ReadInt16()
        {
            return BitConverter.ToInt16(ByteUtils.ReverseArrayIfNeeded(ReadBytes(2)), 0);
        }

        public ushort ReadUInt16()
        {
            return BitConverter.ToUInt16(ByteUtils.ReverseArrayIfNeeded(ReadBytes(2)), 0);
        }

        public float ReadFloat()
        {
            return BitConverter.ToSingle(ByteUtils.ReverseArrayIfNeeded(ReadBytes(2)), 0);
        }

        public int ReadInt32()
        {
            return BitConverter.ToInt32(ByteUtils.ReverseArrayIfNeeded(ReadBytes(4)), 0);
        }

        public uint ReadUInt32()
        {
            return BitConverter.ToUInt32(ByteUtils.ReverseArrayIfNeeded(ReadBytes(4)), 0);
        }

        public long ReadInt64()
        {
            return BitConverter.ToInt64(ByteUtils.ReverseArrayIfNeeded(ReadBytes(8)), 0);
        }

        public ulong ReadUInt64()
        {
            return BitConverter.ToUInt64(ByteUtils.ReverseArrayIfNeeded(ReadBytes(8)), 0);
        }

        public double ReadDouble()
        {
            return BitConverter.ToDouble(ByteUtils.ReverseArrayIfNeeded(ReadBytes(8)), 0);
        }

        public byte[] ReadBytes(int count)
        {
            var result = new byte[2];
            _innerStream.Read(result, 0, count);
            return result;
        }
    }
}