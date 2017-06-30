using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class InsertRequest<T> : InsertReplaceRequest<T>
    {
        public InsertRequest(uint spaceId, T tuple)
            : base(CommandCode.Insert, spaceId, tuple)
        {
        }
    }
}