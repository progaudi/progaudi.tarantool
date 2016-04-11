namespace iproto.Data.Packets
{
    public class ErrorResponse : UnifiedPacket
    {
        public string ErrorMessage { get; }

        public ErrorResponse(Header header, Body body) : base(header, body)
        {
        }
    }
}