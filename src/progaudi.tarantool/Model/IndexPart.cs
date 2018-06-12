using System;
using System.Runtime.Serialization;
using MessagePack;
using MessagePack.Formatters;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackFormatter(typeof(Formatter))]
    public class IndexPart
    {
        public IndexPart(uint fieldNo, FieldType type)
        {
            FieldNo = fieldNo;
            Type = type;
        }

        public uint FieldNo { get; }

        public FieldType Type { get; }

        public override string ToString()
        {
            return $"[{FieldNo}, {Type}]";
        }

        public sealed class Formatter : IMessagePackFormatter<IndexPart>
        {
            public int Serialize(ref byte[] bytes, int offset, IndexPart value, IFormatterResolver formatterResolver)
            {
                throw new NotImplementedException();
            }

            public IndexPart Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                var type = MessagePackBinary.GetMessagePackType(bytes, offset);
                switch (type)
                {
                    case MessagePackType.Nil:
                        readSize = 1;
                        return null;
                    case MessagePackType.Array:
                        return ReadFromArray(bytes, offset, formatterResolver, out readSize);
                    case MessagePackType.Map:
                        return ReadFromMap(bytes, offset, formatterResolver, out readSize);
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }

            private static IndexPart ReadFromMap(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                var startOffset = offset;
                var length = MessagePackBinary.ReadArrayHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                uint? fieldNo = null;
                FieldType? indexPartType = null;

                for (var i = 0; i < length; i++)
                {
                    var key = MessagePackBinary.ReadString(bytes, offset, out readSize);
                    offset += readSize;
                    switch (key)
                    {
                        case "field":
                            fieldNo = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                            offset += readSize;
                            break;
                        case "type":
                            indexPartType = formatterResolver.GetFormatter<FieldType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                            offset += readSize;
                            break;
                        default:
                            offset += MessagePackBinary.ReadNext(bytes, offset);
                            break;
                    }
                }

                readSize = offset - startOffset;

                return fieldNo.HasValue && indexPartType.HasValue
                    ? new IndexPart(fieldNo.Value, indexPartType.Value)
                    : throw new SerializationException("Can't read fieldNo or indexPart from map of index metadata");
            }

            private static IndexPart ReadFromArray(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                var startOffset = offset;
                var length = MessagePackBinary.ReadArrayHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                if (length != 2u)
                {
                    throw ExceptionHelper.InvalidArrayLength(2u, length);
                }

                var fieldNo = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                offset += readSize;
                var indexPartType = formatterResolver.GetFormatter<FieldType>().Deserialize(bytes, offset, formatterResolver, out readSize);
                offset += readSize;

                readSize = offset - startOffset;

                return new IndexPart(fieldNo, indexPartType);
            }
        }
    }
}