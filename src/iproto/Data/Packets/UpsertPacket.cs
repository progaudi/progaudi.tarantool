using System;

using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpsertPacket<T1> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1> tuple, IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1> Tuple { get; }
    }

    public class UpsertPacket<T1, T2> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2> tuple, IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3> tuple, IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3, T4> tuple, IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public UpsertPacket(Header header, int spaceId, Tuple<T1, T2, T3, T4, T5> tuple, IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6> tuple,
            IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6, T7> tuple,
            IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple { get; }
    }

    public class UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public UpsertPacket(
            Header header,
            int spaceId,
            Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple,
            IUpdateOperation updateOperation)
            : base(header)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public IUpdateOperation UpdateOperation { get; }

        public int SpaceId { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Tuple { get; }
    }
}