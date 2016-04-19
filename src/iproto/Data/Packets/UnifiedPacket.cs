using System;
using System.IO;

using iproto.Data.Bodies;
using TarantoolDnx.MsgPack;
using TarantoolDnx.MsgPack.Converters;

namespace iproto.Data.Packets
{
    public class UnifiedPacket
    {
        public UnifiedPacket(Header header, IBody body)
        {
            Header = header;
            Body = body;
        }

        public Header Header { get; }

        public IBody Body { get; }

        public void Serialize(IMsgPackWriter msgPackWriter)
        {
            using (var headerAndBodyWriter = msgPackWriter.Clone())
            {
                Header.WriteTo(headerAndBodyWriter);
                Body.WriteTo(headerAndBodyWriter);
                var headeAndBodyBuffer = headerAndBodyWriter.ToArray();
                msgPackWriter.Write(headeAndBodyBuffer.Length);
                msgPackWriter.WriteRaw(headeAndBodyBuffer);
            }
        }
    }
}