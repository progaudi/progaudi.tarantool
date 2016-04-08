namespace iproto.Data
{
    public class Packet
    {
        public Packet(Header header, Body body)
        {
            Header = header;
            Body = body;
        }

        public Header Header { get; }

        public Body Body { get; } 
    }
}