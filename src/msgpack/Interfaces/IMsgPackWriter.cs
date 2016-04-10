using System.Collections.Generic;

namespace TarantoolDnx.MsgPack.Interfaces
{
    public interface IMsgPackWriter
    {
        void Write(string item);
        void Write(byte[] data);
        void Write<T>(IReadOnlyList<T> data);
        void Write<TK, TV>(IReadOnlyDictionary<TK, TV> map);
        void Write(double item);
        void Write(float item);
        void Write(bool item);
        void Write(byte item);
        void Write(sbyte item);
        void Write(short item);
        void Write(ushort item);
        void Write(int item);
        void Write(uint item);
        void Write(long item);
        void Write(ulong item);
    }
}