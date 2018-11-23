using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class ReplaceRequest<T> : InsertReplaceRequest<T>
    {
        public ReplaceRequest(uint spaceId, T tuple, MsgPackContext context)
            : base(CommandCode.Replace, spaceId, tuple, context)
        {
        }
    }
}