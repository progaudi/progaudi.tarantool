namespace iproto.Data
{
    public class Header
    {
        public Header(int code, int sync, int schemaId)
        {
            Code = code;
            Sync = sync;
            SchemaId = schemaId;
        }

        public int Code { get; }

        public int Sync { get; }

        public int SchemaId { get; }
    }
}