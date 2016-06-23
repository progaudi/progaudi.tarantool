namespace Tarantool.Client.Model.Enums
{
    public enum CommandCode : uint
    {
        //User command codes
        Select = 0x01,
        Insert = 0x02,
        Replace = 0x03,
        Update = 0x04,
        Delete = 0x05,
        Call = 0x06,
        Auth = 0x07,
        Eval = 0x08,
        Upsert = 0x09,

        //Admin command codes
        Ping = 0x40,

        //Response codes
        Ok = 0x00,
        ErrorMask = 0x8000,

        //Replication command codes
        Join = 0x41,
        Subscribe = 0x42,
    }
}