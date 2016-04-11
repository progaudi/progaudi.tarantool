namespace iproto.Data.Packets
{
    public class UnifiedPacket
    {
        public UnifiedPacket(Header header, Body body)
        {
            Header = header;
            Body = body;
        }

        public Header Header { get; }

        public Body Body { get; } 
    }
}