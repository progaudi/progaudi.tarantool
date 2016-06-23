using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model.Requests
{
    public class InsertRequest<T> : InsertReplaceRequest<T>
        where T : ITuple
    {
        public InsertRequest(uint spaceId, T tuple)
            : base(CommandCode.Insert, spaceId, tuple)
        {
        }
    }
}