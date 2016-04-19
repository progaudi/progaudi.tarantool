namespace iproto.Data.Packets
{
    public class ResponsePacket : UnifiedPacket
    {
        public ResponsePacket(Header header, string errorMessage, object data) : base(header)
        {
            ErrorMessage = errorMessage;
            Data = data;
        }

        public string ErrorMessage { get; }

        public object Data { get; }
    }
}