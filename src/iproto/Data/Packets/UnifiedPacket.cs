using System;
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

        public byte[] Serialize(MsgPackContext msgPackContext)
        {
            var serializedHeader = Header.Serialize(msgPackContext);
            var serializedBody = Body.Serialize(msgPackContext);
            var serializedLength = MsgPackConverter.Serialize(serializedBody.Length + serializedHeader.Length);

            var result = new byte[serializedLength.Length + serializedBody.Length + serializedHeader.Length];

            Array.Copy(serializedLength, result, serializedLength.Length);
            Array.Copy(serializedHeader, 0, result, serializedLength.Length, serializedHeader.Length);
            Array.Copy(serializedBody, 0, result, serializedLength.Length + serializedHeader.Length, serializedBody.Length);

            return result;
        }
    }
}