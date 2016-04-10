namespace TarantoolDnx.MsgPack
{
    internal static class IntLimits
    {
        public const int Max4Bit = 0xf;
        public const int Max5Bit = 0x1f;
        public const int Max7Bit = 0x7f;
        public const int Max8Bit = 0xff;
        public const int Max15Bit = 0x7fff;
        public const int Max16Bit = 0xffff;
        public const int Max31Bit = 0x7fffffff;
        public const long Max32Bit = 0xffffffffL;
    }
}