using System;

namespace TarantoolDnx.MsgPack
{
    public interface IBytesWriter : IDisposable
    {
        void Write(DataTypes dataType);

        void Write(byte value);

        void Write(byte[] array);
    }
}
