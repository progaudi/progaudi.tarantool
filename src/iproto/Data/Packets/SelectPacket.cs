using System;

namespace iproto.Data.Packets
{
    public class SelectPacket<T> : UnifiedPacket
    {
        public SelectPacket(Header header, int spaceId, int indexId, int limit, int offset, Iterator iterator, Tuple<T> selectKey)
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

        public Tuple<T> SelectKey { get; }
    }
}