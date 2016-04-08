namespace TarantoolDnx.MsgPack.Interfaces
{
    public interface IBytesWriter
    {
        void Write(byte data);
        void Write(sbyte data);
        void Write(byte[] data);
        void WriteBigEndianBytes(double data);
        void WriteBigEndianBytes(float data);
        void WriteBigEndianBytes(short data);
        void WriteBigEndianBytes(ushort data);
        void WriteBigEndianBytes(int data);
        void WriteBigEndianBytes(uint data);
        void WriteBigEndianBytes(long data);
        void WriteBigEndianBytes(ulong data);
    }
}