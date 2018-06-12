using MessagePack;
using MessagePack.Formatters;
using static MessagePack.MessagePackBinary;

namespace ProGaudi.Tarantool.Client.Model
{
    public class SelectRequest<T> : IRequest
    {
        public SelectRequest(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, T selectKey)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public T SelectKey { get; }

        public CommandCodes Code => CommandCodes.Select;

        public sealed class Formatter : IMessagePackFormatter<SelectRequest<T>>
        {
            public int Serialize(ref byte[] bytes, int offset, SelectRequest<T> value, IFormatterResolver formatterResolver)
            {
                var startOffset = offset;

                offset += WriteFixedMapHeaderUnsafe(ref bytes, offset, 6);
                offset += WriteUInt32(ref bytes, offset, Keys.SpaceId);
                offset += WriteUInt32(ref bytes, offset, value.SpaceId);
                offset += WriteUInt32(ref bytes, offset, Keys.IndexId);
                offset += WriteUInt32(ref bytes, offset, value.IndexId);
                offset += WriteUInt32(ref bytes, offset, Keys.Limit);
                offset += WriteUInt32(ref bytes, offset, value.Limit);
                offset += WriteUInt32(ref bytes, offset, Keys.Offset);
                offset += WriteUInt32(ref bytes, offset, value.Offset);
                offset += WriteUInt32(ref bytes, offset, Keys.Iterator);
                offset += WriteUInt32(ref bytes, offset, (uint)value.Iterator);
                offset += WriteUInt32(ref bytes, offset, Keys.Key);
                offset += formatterResolver
                    .GetFormatter<T>()
                    .Serialize(ref bytes, offset, value.SelectKey, formatterResolver);

                return offset - startOffset;
            }

            public SelectRequest<T> Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}