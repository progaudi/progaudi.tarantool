namespace Tarantool.Client.IProto.Data
{
    public class RequestHeader : HeaderBase
    {
        public RequestHeader(CommandCode code, RequestId requestId)
            : base(code, requestId)
        {
        }
    }
}