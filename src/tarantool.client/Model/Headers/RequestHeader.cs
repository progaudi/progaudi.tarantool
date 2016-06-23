using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model.Headers
{
    public class RequestHeader : HeaderBase
    {
        public RequestHeader(CommandCode code, RequestId requestId)
            : base(code, requestId)
        {
        }
    }
}