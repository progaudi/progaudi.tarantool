using System.Collections.Generic;

namespace MsgPackLite.Interfaces
{
    public interface IMsgPackReader
    {
        string ReadString();
        double ReadDouble();
        float ReadFloat();
        bool ReadBool();
        byte ReadByte();
        sbyte ReadSByte();
        short ReadShort();
        ushort ReadUShort();
        int ReadInt();  
        uint ReadUInt();
        long ReadLong();
        ulong ReadULong();
        byte[] ReadBinary();
        T[] ReadArray<T>();
        IDictionary<TK, TV> ReadMap<TK, TV>();
    }
}