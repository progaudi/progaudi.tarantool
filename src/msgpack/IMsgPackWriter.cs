using System;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackWriter : IDisposable
    {
        void Write<T>(T value);

        byte[] ToArray();

        IMsgPackWriter Clone();

        void WriteRaw(byte[] headeAndBodyBuffer);
    }
}