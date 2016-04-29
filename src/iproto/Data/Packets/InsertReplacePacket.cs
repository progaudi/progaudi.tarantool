namespace iproto.Data.Packets
{
    public class InsertReplacePacket<T> : UnifiedPacket
        where T : ITuple
    {
        public InsertReplacePacket(CommandCode code, uint spaceId, T tuple)
            : base(new Header(code, null, null))
        {
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public uint SpaceId { get; }

        public T Tuple { get; }
    }
}