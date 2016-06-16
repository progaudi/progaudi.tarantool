namespace Tarantool.Client.IProto.Data
{
    public class ResponseHeader : HeaderBase
    {
        public ResponseHeader(CommandCode code, ulong requestId, ulong schemaId) : base(code, requestId)
        {
            SchemaId = schemaId;
        }

        public ulong SchemaId { get; }
    }
}