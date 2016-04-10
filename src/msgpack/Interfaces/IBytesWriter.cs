namespace TarantoolDnx.MsgPack.Interfaces
{
    internal interface IBytesWriter
    {
        void Write(DataTypes data);
        void Write(byte data);
        void Write(sbyte data);
        void Write(byte[] data);
        void Write(double data);
        void Write(float data);
        void Write(short data);
        void Write(ushort data);
        void Write(int data);
        void Write(uint data);
        void Write(long data);
        void Write(ulong data);
    }
}