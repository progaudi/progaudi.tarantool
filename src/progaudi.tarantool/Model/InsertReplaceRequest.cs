using MessagePack;
using MessagePack.Formatters;
using static MessagePack.MessagePackBinary;

namespace ProGaudi.Tarantool.Client.Model
{
    public abstract class InsertReplaceRequest<T> : IRequest
    {
        protected InsertReplaceRequest(CommandCodes code, uint spaceId, T tuple)
        {
            Code = code;
            SpaceId = spaceId;
            Tuple = tuple;
        }

        public uint SpaceId { get; }

        public T Tuple { get; }

        public CommandCodes Code { get; }

        public sealed class Formatter : IMessagePackFormatter<InsertReplaceRequest<T>>
        {
            public int Serialize(ref byte[] bytes, int offset, InsertReplaceRequest<T> value, IFormatterResolver formatterResolver)
            {
                var startOffset = offset;

                offset += WriteFixedMapHeaderUnsafe(ref bytes, offset, 2);
                offset += WriteUInt32(ref bytes, offset, Keys.SpaceId);
                offset += WriteUInt32(ref bytes, offset, value.SpaceId);
                offset += WriteUInt32(ref bytes, offset, Keys.Tuple);
                offset += formatterResolver.GetFormatter<T>().Serialize(ref bytes, offset, value.Tuple, formatterResolver);

                return offset - startOffset;
            }

            public InsertReplaceRequest<T> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}
