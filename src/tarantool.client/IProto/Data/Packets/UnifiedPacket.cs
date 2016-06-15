namespace Tarantool.Client.IProto.Data.Packets
{
    public abstract class UnifiedPacket
    {
        protected UnifiedPacket(Header header)
        {
            Header = header;
        }

        public Header Header { get; }
    }
}