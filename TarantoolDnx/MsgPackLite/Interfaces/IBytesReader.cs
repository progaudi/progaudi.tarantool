namespace MsgPackLite.Interfaces
{
    public interface IBytesReader
    {
        sbyte ReadSByte();
        byte ReadByte();
        short ReadInt16();
        ushort ReadUInt16();
        int ReadInt32();
        uint ReadUInt32();
        long ReadInt64();
        ulong ReadUInt64();
        double ReadDouble();
        float ReadFloat();
        byte[] ReadBytes(int count);
    }
}