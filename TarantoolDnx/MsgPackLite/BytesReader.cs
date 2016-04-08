using System.IO;

namespace MsgPackLite
{
    internal sealed class BytesReader :IBytesReader
    {
        private readonly Stream _innerStream;

        public BytesReader(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public sbyte ReadSByte()
        {
            throw new System.NotImplementedException();
        }

        public byte ReadByte()
        {
            return (byte)_innerStream.ReadByte();
        }

        short IBytesReader.ReadInt16()
        {
            throw new System.NotImplementedException();
        }

        public ushort ReadUInt16()
        {
            throw new System.NotImplementedException();
        }

        public float ReadFloat()
        {
            throw new System.NotImplementedException();
        }

        public void ReadBytes(byte[] data, int size)
        {
            _innerStream.Read(data, 0, size);
        }

        public int ReadInt16()
        {
            throw new System.NotImplementedException();
        }

        public int ReadInt32()
        {
            throw new System.NotImplementedException();
        }

        public uint ReadUInt32()
        {
            throw new System.NotImplementedException();
        }

        public long ReadInt64()
        {
            throw new System.NotImplementedException();
        }

        public ulong ReadUInt64()
        {
            throw new System.NotImplementedException();
        }

        public double ReadDouble()
        {
            throw new System.NotImplementedException();
        }
    }
}