namespace TarantoolDnx.MsgPack
{
    public static class DataTypesExtension
    {
        public static byte GetHighBits(this DataTypes type, byte bitsCount)
        {
            return (byte)((byte)type >> (8 - bitsCount));
        }
    }
}