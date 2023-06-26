namespace ProGaudi.Tarantool.Client.Model.Enums
{
    // probably the best decision is to move these values into DataTypes enum in MsgPack.Light.dll
    // for now I decided not to touch it
    internal class MsgPackExtDataTypes
    {
        public const byte Ext8 = 0xc7;
        public const byte Ext16 = 0xc8;
        public const byte Ext32 = 0xc9;
        public const byte FixExt1 = 0xd4;
        public const byte FixExt2 = 0xd5;
        public const byte FixExt4 = 0xd6;
        public const byte FixExt8 = 0xd7;
        public const byte FixExt16 = 0xd8;
    }
}
