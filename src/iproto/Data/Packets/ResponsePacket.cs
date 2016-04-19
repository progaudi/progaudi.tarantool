namespace iproto.Data.Packets
{
    public class ResponsePacket
    {
        public ResponsePacket(Header header, string errorMessage, object data)
        {
            Header = header;
            ErrorMessage = errorMessage;
            Data = data;
        }

        public Header Header { get; }

        public string ErrorMessage { get; }

        public object Data { get; }
    }
}