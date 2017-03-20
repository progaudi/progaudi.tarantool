using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Headers
{
    public class ResponseHeader : HeaderBase
    {
        public ResponseHeader(CommandCode code, RequestId requestId, ulong? schemaId) : base(code, requestId)
        {
            SchemaId = schemaId;
        }

        public ulong? SchemaId { get; }
    }
}