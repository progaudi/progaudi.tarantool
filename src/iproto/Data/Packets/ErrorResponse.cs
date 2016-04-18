using iproto.Data.Bodies;

namespace iproto.Data.Packets
{
    public class ErrorResponse : UnifiedPacket
    {
        public string ErrorMessage { get; }

        public ErrorResponse(Header header, IBody body) : base(header, body)
        {
        }
    }
}