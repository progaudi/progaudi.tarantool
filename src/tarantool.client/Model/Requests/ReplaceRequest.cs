using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model.Requests
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