using System;

using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpsertPacket<T1, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1> tuple, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2> tuple, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3> tuple, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3, T4> tuple, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5> tuple, UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6> tuple,
            UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6, T7> tuple,
            UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple,
            UpdateOperation<TUpdate> updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Tuple { get; }
    }
}