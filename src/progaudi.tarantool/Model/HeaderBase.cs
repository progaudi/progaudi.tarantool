namespace ProGaudi.Tarantool.Client.Model
{
    public abstract class HeaderBase
    {
        protected HeaderBase(CommandCodes code, RequestId requestId)
        {
            Code = code;
            RequestId = requestId;
        }

        public CommandCodes Code { get; }

        public RequestId RequestId { get; set; }
    }
}