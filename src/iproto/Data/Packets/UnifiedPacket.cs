namespace iproto.Data.Packets
{
    public class UnifiedPacket
    {
        public UnifiedPacket(Header header)
        {
            Header = header;
        }

        public Header Header { get; }
    }
}