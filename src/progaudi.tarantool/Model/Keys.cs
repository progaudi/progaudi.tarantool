namespace ProGaudi.Tarantool.Client.Model
{
    public static class Keys
    {
        // Request keys
        public const uint Code = 0x00;
        public const uint Sync = 0x01;
        public const uint SchemaId = 0x05;
        public const uint SpaceId = 0x10;
        public const uint IndexId = 0x11;
        public const uint Limit = 0x12;
        public const uint Offset = 0x13;
        public const uint Iterator = 0x14;
        public const uint Key = 0x20;
        public const uint Tuple = 0x21;
        public const uint FunctionName = 0x22;
        public const uint Username = 0x23;
        public const uint Expression = 0x27;
        public const uint Ops = 0x28;
        public const uint FieldName = 0x29;

        // Response keys
        public const uint Data = 0x30;
        public const uint Error = 0x31;
        public const uint Metadata = 0x32;

        // Sql keys
        public const uint SqlQueryText = 0x40;
        public const uint SqlParameters = 0x41;
        public const uint SqlOptions = 0x42;
        public const uint SqlInfo = 0x43;
        public const uint SqlRowCount = 0x44;

        // Replication keys
        public const uint ServerId = 0x02;
        public const uint Lsn = 0x03;
        public const uint Timestamp = 0x04;
        public const uint ServerUuid = 0x24;
        public const uint ClusterUuid = 0x25;
        public const uint Vclock = 0x26;
    }
}