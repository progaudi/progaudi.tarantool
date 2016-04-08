namespace MsgPackLite
{
    public sealed class MsgPackConstants
    {
        public const int Max4Bit = 0xf;
        public const int Max5Bit = 0x1f;
        public const int Max7Bit = 0x7f;
        public const int Max8Bit = 0xff;
        public const int Max15Bit = 0x7fff;
        public const int Max16Bit = 0xffff;
        public const int Max31Bit = 0x7fffffff;
        public const long Max32Bit = 0xffffffffL;
        public const byte MpNull = 0xc0;
        public const byte MpFalse = 0xc2;
        public const byte MpTrue = 0xc3;
        public const byte MpFloat = 0xca;
        public const byte MpDouble = 0xcb;
        public const byte MpUint8 = 0xcc;
        public const byte MpUint16 = 0xcd;
        public const byte MpUint32 = 0xce;
        public const byte MpUint64 = 0xcf;
        public const byte MpNegativeFixnum = 0xe0; //last 5 bits is value
        public const byte MpInt8 = 0xd0;
        public const byte MpInt16 = 0xd1;
        public const byte MpInt32 = 0xd2;
        public const byte MpInt64 = 0xd3;
        public const byte MpFixarray = 0x90; //last 4 bits is size
        public const byte MpArray16 = 0xdc;
        public const byte MpArray32 = 0xdd;
        public const byte MpFixmap = 0x80; //last 4 bits is size
        public const byte MpMap16 = 0xde;
        public const byte MpMap32 = 0xdf;
        public const byte MpFixstr = 0xa0; //last 5 bits is size
        public const byte MpStr8 = 0xd9;
        public const byte MpStr16 = 0xda;
        public const byte MpStr32 = 0xdb;
        public const byte MpBit8 = 0xc4;
        public const byte MpBit16 = 0xc5;
        public const byte MpBit32 = 0xc6;
    }
}