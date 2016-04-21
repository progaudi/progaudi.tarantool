using System;

namespace iproto.Data.Packets
{
    public class DeletePacket<T> :UnifiedPacket
    {
        public DeletePacket(Header header, int spaceId, int indexId, Tuple<T> key)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public Tuple<T> Key { get; }
    }
}