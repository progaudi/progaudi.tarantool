namespace TarantoolDnx.MsgPack
{
    public enum DataTypes : byte
    {
        Null = 0xc0,
        False = 0xc2,
        True = 0xc3,
        Single = 0xca,
        Double = 0xcb,
        UInt8 = 0xcc,
        UInt16 = 0xcd,
        UInt32 = 0xce,
        UInt64 = 0xcf,
        NegativeFixnum = 0xe0, //last 5 bits is value
        Int8 = 0xd0,
        Int16 = 0xd1,
        Int32 = 0xd2,
        Int64 = 0xd3,
        FixArray = 0x90, //last 4 bits is size
        Array16 = 0xdc,
        Array32 = 0xdd,
        FixMap = 0x80, //last 4 bits is size
        Map16 = 0xde,
        Map32 = 0xdf,
        FixStr = 0xa0, //last 5 bits is size
        Str8 = 0xd9,
        Str16 = 0xda,
        Str32 = 0xdb,
        Bin8 = 0xc4,
        Bin16 = 0xc5,
        Bin32 = 0xc6
    }
}