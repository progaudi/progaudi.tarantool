using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Headers
{
    public abstract class HeaderBase
    {
        protected HeaderBase(CommandCode code, RequestId requestId)
        {
            Code = code;
            RequestId = requestId;
        }

        public CommandCode Code { get; }

        public RequestId RequestId { get; }
    }
}