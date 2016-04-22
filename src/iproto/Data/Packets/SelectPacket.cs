using System;

namespace iproto.Data.Packets
{
    public class SelectPacket<T1> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1> SelectKey { get; }
    }

    public class SelectPacket<T1, T2> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2, T3> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2, T3, T4> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5, T6> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5, T6, T7> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> SelectKey { get; }
    }

    public class SelectPacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> selectKey)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public int Limit { get; }

        public int Offset { get; }

        public Iterator Iterator { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> SelectKey { get; }
    }
}