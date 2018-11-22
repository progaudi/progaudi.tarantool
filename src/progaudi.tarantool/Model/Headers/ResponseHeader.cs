using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Headers
{
    public class ResponseHeader : HeaderBase
    {
        public ResponseHeader(CommandCode code, RequestId id, ulong? schemaId) : base(code, id)
        {
            SchemaId = schemaId;
        }

        public ulong? SchemaId { get; }
    }
}