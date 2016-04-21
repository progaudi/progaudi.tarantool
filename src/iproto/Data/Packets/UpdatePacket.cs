using System;

using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpdatePacket<T1, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }
}