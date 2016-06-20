namespace Tarantool.Client.IProto.Data.Packets
{
    public class DeletePacket<T> : IRequestPacket
        where T : ITuple
    {
        public DeletePacket(uint spaceId, uint indexId, T key)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public T Key { get; }

        public CommandCode Code => CommandCode.Delete;
    }
}