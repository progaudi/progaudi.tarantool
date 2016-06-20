namespace Tarantool.Client.IProto.Data.Packets
{
    public class ReplacePacket<T> : InsertReplacePacket<T>
        where T : ITuple
    {
        public ReplacePacket(uint spaceId, T tuple)
            : base(CommandCode.Insert, spaceId, tuple)
        {
        }
    }
}