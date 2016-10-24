using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class ReplaceRequest<T> : InsertReplaceRequest<T>
        where T : ITuple
    {
        public ReplaceRequest(uint spaceId, T tuple)
            : base(CommandCode.Replace, spaceId, tuple)
        {
        }
    }
}