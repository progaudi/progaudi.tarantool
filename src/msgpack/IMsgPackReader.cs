using System;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackReader : IDisposable
    {
        T Read<T>();
    }
}