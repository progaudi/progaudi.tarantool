namespace iproto.Data
{
    public class Header
    {
        public Header(CommandCode code, ulong sync, ulong schemaId)
        {
            Code = code;
            Sync = sync;
            SchemaId = schemaId;
        }

        public CommandCode Code { get; }

        public ulong Sync { get; }

        public ulong SchemaId { get; }
    }
}