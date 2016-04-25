using System;

namespace iproto.Data.Packets
{
    public class SelectPacket<T1> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1> SelectKey { get; }
    }

    public class SelectPacket<T1, T2> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2, T3> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2, T3, T4> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5, T6> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5, T6, T7> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> selectKey)
            : base(new Header(CommandCode.Select, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> SelectKey { get; }
    }
}