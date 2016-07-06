using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Model.Requests
{
    public class UpdateRequest<TKey> : IRequest
     where TKey : ITuple
    {
        public UpdateRequest(uint spaceId, uint indexId, TKey key, UpdateOperation[] updateOperations)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperations = updateOperations;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public TKey Key { get; }

        public UpdateOperation[] UpdateOperations { get; }

        public CommandCode Code => CommandCode.Update;
    }
}