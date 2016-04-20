using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    public interface IBytesReader : IDisposable
    {
        DataTypes ReadDataType();

        byte ReadByte();

        void ReadBytes(byte[] buffer);

        void Seek(int offset, SeekOrigin origin);

        bool ReadArrayLengthOrNull(out uint length);

        bool ReadMapLengthOrNull(out uint length);
    }
}