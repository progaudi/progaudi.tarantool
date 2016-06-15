namespace Tarantool.Client.IProto.Data.Packets
{
    public class ResponsePacket<T> : UnifiedPacket
    {
        public ResponsePacket(Header header, string errorMessage, T data) : base(header)
        {
            ErrorMessage = errorMessage;
            Data = data;
        }

        public string ErrorMessage { get; }

        public T Data { get; }
    }


}