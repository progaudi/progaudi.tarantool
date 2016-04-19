using System;

using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpsertPacket<T1, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1> tuple, UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2> tuple, UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3> tuple, UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3, T4> tuple, UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5> tuple, UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6> tuple,
            UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6, T7> tuple,
            UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdateOperation> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple,
            UpdateOperation<TUpdateOperation> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdateOperation> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Tuple { get; }
    }
}