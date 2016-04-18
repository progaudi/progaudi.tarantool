using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackReader : IDisposable
    {
        DataTypes ReadDataType();

        byte ReadByte();

        void ReadBytes(byte[] buffer);

        void Seek(int offset, SeekOrigin origin);
    }
}