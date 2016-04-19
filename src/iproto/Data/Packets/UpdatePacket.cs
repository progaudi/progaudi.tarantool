using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpdatePacket<TSelect, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, SelectKey<TSelect> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public SelectKey<TSelect> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }
}