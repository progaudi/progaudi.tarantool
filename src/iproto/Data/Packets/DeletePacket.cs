namespace iproto.Data.Packets
{
    public class DeletePacket<T> : UnifiedPacket
        where T : IMyTuple
    {
        public DeletePacket(uint spaceId, uint indexId, T key)
            : base(new Header(CommandCode.Delete, null, null))
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public T Key { get; }
    }
}