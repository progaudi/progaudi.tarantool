using iproto.Data.Bodies;

namespace iproto.Data.Packets
{
    public class OkResponse<T>: UnifiedPacket
    {
        public T Data { get; }

        internal OkResponse(Header header, IBody body) : base(header, body)
        {
        }
    }
}