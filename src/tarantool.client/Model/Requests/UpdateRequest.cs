using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Model.Requests
{
    public class UpdateRequest<TKey, TUpdate> : IRequest
     where TKey : ITuple
    {
        public UpdateRequest(uint spaceId, uint indexId, TKey key, UpdateOperation<TUpdate> updateOperation)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public TKey Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public CommandCode Code => CommandCode.Update;
    }
}