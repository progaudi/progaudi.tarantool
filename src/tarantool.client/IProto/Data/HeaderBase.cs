namespace Tarantool.Client.IProto.Data
{
    public abstract class HeaderBase
    {
        protected HeaderBase(CommandCode code, ulong requestId)
        {
            Code = code;
            RequestId = requestId;
        }

        public CommandCode Code { get; }

        public ulong RequestId { get; set; }
    }
}