namespace tarantool_client.IProto.Data.Packets
{
    public class SelectPacket<T> : UnifiedPacket
        where T : ITuple
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, T selectKey)
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

        public T SelectKey { get; }
    }
}