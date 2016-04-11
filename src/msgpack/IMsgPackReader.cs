using System.IO;

namespace TarantoolDnx.MsgPack
{
    public interface IMsgPackReader
    {
        DataTypes ReadDataType();

        byte ReadByte();

        void ReadBytes(byte[] buffer);

        void Seek(int offset, SeekOrigin origin);
    }
}