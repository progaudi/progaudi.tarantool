using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public abstract class InsertReplaceRequest<T> : Request
    {
        protected InsertReplaceRequest(CommandCode code, uint spaceId, T tuple, MsgPackContext context)
            : base(code, context)
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public uint SpaceId { get; }

        public T Tuple { get; }
    }
}