namespace Tarantool.Client.IProto.Data.Packets
{
    public abstract class InsertReplacePacket<T> : IRequestPacket
        where T : ITuple
    {
        protected InsertReplacePacket(CommandCode code, uint spaceId, T tuple)
        {
            Code = code;
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public uint SpaceId { get; }

        public T Tuple { get; }

        public CommandCode Code { get; }
    }
}