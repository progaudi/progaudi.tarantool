using MessagePack;
using MessagePack.Formatters;
using static MessagePack.MessagePackBinary;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackFormatter(typeof(Formatter))]
    public class RequestHeader : HeaderBase
    {
        public RequestHeader(CommandCodes code, RequestId requestId)
            : base(code, requestId)
        {
        }

        public sealed class Formatter : IMessagePackFormatter<RequestHeader>
        {
            public int Serialize(ref byte[] bytes, int offset, RequestHeader value, IFormatterResolver formatterResolver)
            {
                var startOffset = offset;

                offset += WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
                offset += WriteUInt32(ref bytes, offset, Keys.Code);
                offset += WriteUInt32(ref bytes, offset, (uint)value.Code);
                offset += WriteUInt32(ref bytes, offset, Keys.Sync);
                offset += WriteUInt64(ref bytes, offset, value.RequestId.Value);

                return offset - startOffset;
            }

            public RequestHeader Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}