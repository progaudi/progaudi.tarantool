namespace Tarantool.Client.IProto.Data.Packets
{
    public class InsertPacket<T> : InsertReplacePacket<T>
        where T : ITuple
    {
        public InsertPacket(uint spaceId, T tuple)
            : base(CommandCode.Insert, spaceId, tuple)
        {
        }
    }
}