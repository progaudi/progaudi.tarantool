using Tarantool.Client.IProto.Data.UpdateOperations;

namespace Tarantool.Client.IProto.Data.Packets
{
    public class UpdatePacket<TKey, TUpdate> : IRequestPacket
     where TKey : ITuple
    {
        public UpdatePacket(uint spaceId, uint indexId, TKey key, UpdateOperation<TUpdate> updateOperation)
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