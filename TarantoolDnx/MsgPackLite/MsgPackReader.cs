using System.Collections.Generic;
using System.IO;

namespace MsgPackLite
{
    public class MsgPackReader:IMsgPackReader
    {
        private readonly Stream _innerStream;

        public MsgPackReader(Stream innerStream)
        {
            _innerStream = innerStream;
        }

        public string ReadString()
        {
            throw new System.NotImplementedException();
        }

        public double ReadDouble()
        {
            throw new System.NotImplementedException();
        }

        public float ReadFloat()
        {
            throw new System.NotImplementedException();
        }

        public bool ReadBool()
        {
            throw new System.NotImplementedException();
        }

        public byte ReadByte()
        {
            throw new System.NotImplementedException();
        }

        public sbyte ReadSByte()
        {
            throw new System.NotImplementedException();
        }

        public short ReadShort()
        {
            throw new System.NotImplementedException();
        }

        public ushort ReadUShort()
        {
            throw new System.NotImplementedException();
        }

        public int ReadInt()
        {
            throw new System.NotImplementedException();
        }

        public uint ReadUInt()
        {
            throw new System.NotImplementedException();
        }

        public long ReadLong()
        {
            throw new System.NotImplementedException();
        }

        public ulong ReadULong()
        {
            throw new System.NotImplementedException();
        }

        public byte[] ReadBinary()
        {
            throw new System.NotImplementedException();
        }

        public T[] ReadArray<T>()
        {
            throw new System.NotImplementedException();
        }

        public IDictionary<TK, TV> ReadDictionary<TK, TV>()
        {
            throw new System.NotImplementedException();
        }
    }
}