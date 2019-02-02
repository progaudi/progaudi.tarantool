using System.Buffers;
using System.Runtime.Serialization;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class IndexPartParser : IMsgPackSequenceParser<IndexPart>
    {
        public IndexPart Parse(ReadOnlySequence<byte> source, out int readSize)
        {
            if (MsgPackSpec.TryReadMapHeader(source, out var mapLength, out readSize))
            {
                return ReadFromMap(source, mapLength, ref readSize);
            }

            if (MsgPackSpec.TryReadArrayHeader(source, out var arrayHeader, out readSize))
            {
                return ReadFromArray(source, arrayHeader, ref readSize);
            }

            throw ExceptionHelper.CodeIsNotArrayOrMap(source.GetFirst());
        }

        private static IndexPart ReadFromArray(in ReadOnlySequence<byte> source, int length, ref int readSize)
        {
            if (length != 2u)
            {
                throw ExceptionHelper.InvalidArrayLength(2u, length);
            }

            var fieldNo = MsgPackSpec.ReadUInt32(source.Slice(readSize), out var temp);
            readSize += temp;
            var indexPartType = MsgPackSpec.ReadString(source.Slice(readSize), out temp);
            readSize += temp;

            return new IndexPart(fieldNo, indexPartType);
        }

        private static IndexPart ReadFromMap(in ReadOnlySequence<byte> source, int length, ref int readSize)
        {
            uint? fieldNo = null;
            string indexPartType = null;

            for (var i = 0; i < length; i++)
            {
                var name = MsgPackSpec.ReadString(source.Slice(readSize), out var temp);
                readSize += temp;
                switch (name)
                {
                    case "field":
                        fieldNo = MsgPackSpec.ReadUInt32(source.Slice(readSize), out temp);
                        readSize += temp;
                        break;
                    case "type":
                        indexPartType = MsgPackSpec.ReadString(source.Slice(readSize), out temp);
                        readSize += temp;
                        break;
                    default:
                        readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).GetIntLength();
                        break;
                }
            }

            if (fieldNo.HasValue && indexPartType != null)
            {
                return new IndexPart(fieldNo.Value, indexPartType);
            }

            throw new SerializationException("Can't read fieldNo or indexPart from map of index metadata");
        }
    }
}
