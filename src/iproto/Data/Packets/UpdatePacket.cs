using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpdatePacket<TKey, TUpdate> : UnifiedPacket
     where TKey : IMyTuple
    {
        public UpdatePacket(uint spaceId, uint indexId, TKey key, UpdateOperation<TUpdate> updateOperation)
            : base(new Header(CommandCode.Update, null, null))
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
    }
}