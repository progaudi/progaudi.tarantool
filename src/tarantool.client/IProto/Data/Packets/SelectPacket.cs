namespace Tarantool.Client.IProto.Data.Packets
{
    public class SelectPacket<T> : IRequestPacket
        where T : ITuple
    {
        public SelectPacket(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, T selectKey)
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

        public CommandCode Code => CommandCode.Select;
    }
}