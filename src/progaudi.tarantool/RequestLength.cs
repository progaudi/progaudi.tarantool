using MessagePack;
using MessagePack.Formatters;

namespace ProGaudi.Tarantool.Client
{
    [MessagePackFormatter(typeof(Formatter))]
    public readonly struct RequestLength
    {
        private readonly uint _value;

        public RequestLength(byte[] header, byte[] body) => _value = (uint) (header.Length + body.Length);

        public RequestLength(uint length) => _value = length;

        public static implicit operator uint(RequestLength length)
        {
            return length._value;
        }

        public sealed class Formatter : IMessagePackFormatter<RequestLength>
        {
            public int Serialize(ref byte[] bytes, int offset, RequestLength value, IFormatterResolver formatterResolver)
            {
                return MessagePackBinary.WriteUInt32ForceUInt32Block(ref bytes, offset, value);
            }

            public RequestLength Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                return new RequestLength(MessagePackBinary.ReadUInt32(bytes, offset, out readSize));
            }
        }
    }
}