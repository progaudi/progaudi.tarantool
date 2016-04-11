namespace iproto.Data.Packets
{
    public class OkResponse<T>: UnifiedPacket
    {
        public T Data { get; }

        internal OkResponse(Header header, Body body) : base(header, body)
        {
        }
    }
}