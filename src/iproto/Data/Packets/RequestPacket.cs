using iproto.Data.Request;

using TarantoolDnx.MsgPack;

namespace iproto.Data.Packets
{
    public class RequestPacket
    {
        public RequestPacket(Header header, IRequestBody requestBody)
        {
            Header = header;
            RequestBody = requestBody;
        }

        public Header Header { get; }

        public IRequestBody RequestBody { get; }

        public void Serialize(IMsgPackWriter msgPackWriter)
        {
            using (var headerAndBodyWriter = msgPackWriter.Clone())
            {
                Header.WriteTo(headerAndBodyWriter);
                RequestBody.WriteTo(headerAndBodyWriter);
                var headeAndBodyBuffer = headerAndBodyWriter.ToArray();
                msgPackWriter.Write(headeAndBodyBuffer.Length);
                msgPackWriter.WriteRaw(headeAndBodyBuffer);
            }
        }
    }
}