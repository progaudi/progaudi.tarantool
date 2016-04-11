namespace iproto.Data
{
    public class Header
    {
        public Header(CommandCode code, int sync, int schemaId)
        {
            Code = code;
            Sync = sync;
            SchemaId = schemaId;
        }

        public CommandCode Code { get; }

        public int Sync { get; }

        public int SchemaId { get; }
    }
}