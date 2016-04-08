namespace iproto.Data
{
    public class UserCommandCodes
    {
        public const int Select = 0x01;

        public const int Insert = 0x02;

        public const int Replace = 0x03;

        public const int Update = 0x04;

        public const int Delete = 0x05;

        public const int Call = 0x06;

        public const int Auth = 0x07;

        public const int Eval = 0x08;

        public const int Upsert = 0x09;

        public const int Ping = 0x40;

        public const int Ok = 0x00;

        public const int ErrorMask = 0x8000;
    }
}