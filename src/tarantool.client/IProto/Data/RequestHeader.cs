namespace Tarantool.Client.IProto.Data
{
    public class RequestHeader : HeaderBase
    {
        public RequestHeader(CommandCode code, ulong requestId)
            : base(code, requestId)
        {
        }
    }
}