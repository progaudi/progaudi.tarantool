namespace iproto.Data.Packets
{
    public class DeletePacket<T> :UnifiedPacket
    {
        public DeletePacket(Header header, int spaceId, int indexId, SelectKey<T> key)
            : base(header)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public int SpaceId { get; }

        public int IndexId { get; }

        public SelectKey<T> Key { get; }
    }
}