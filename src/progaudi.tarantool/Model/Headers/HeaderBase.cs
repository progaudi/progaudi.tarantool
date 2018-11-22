using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Headers
{
    public abstract class HeaderBase
    {
        protected HeaderBase(CommandCode code, RequestId id)
        {
            Code = code;
            Id = id;
        }

        public CommandCode Code { get; }

        public RequestId Id { get; }
    }
}