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

    public class UpdatePacket<T1, T2, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }

    public class UpdatePacket<T1, T2, T3, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2, T3> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2, T3> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }

    public class UpdatePacket<T1, T2, T3, T4, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2, T3, T4> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2, T3, T4> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }

    public class UpdatePacket<T1, T2, T3, T4, T5, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2, T3, T4, T5> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }

    public class UpdatePacket<T1, T2, T3, T4, T5, T6, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2, T3, T4, T5, T6> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }

    public class UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2, T3, T4, T5, T6, T7> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }

    public class UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> : UnifiedPacket
    {
        public UpdatePacket(Header header, int spaceId, int indexId, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> key, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperation = updateOperation;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Key { get; }

        public UpdateOperation<TUpdate> UpdateOperation { get; }
    }
}