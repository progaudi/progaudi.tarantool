using MessagePack;
using MessagePack.Formatters;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackFormatter(typeof(Formatter))]
    public class ResponseHeader : HeaderBase
    {
        public ResponseHeader(CommandCodes code, RequestId requestId, ulong? schemaId) : base(code, requestId)
        {
            SchemaId = schemaId;
        }

        public ulong? SchemaId { get; }

        public sealed class Formatter : IMessagePackFormatter<ResponseHeader>
        {
            public int Serialize(ref byte[] bytes, int offset, ResponseHeader value, IFormatterResolver formatterResolver)
            {
                throw new System.NotImplementedException();
            }

            public ResponseHeader Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                var startOffset = offset;
                var length = MessagePackBinary.ReadMapHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                CommandCodes? code = null;
                RequestId? sync = null;
                ulong? schemaId = null;

                for (var i = 0; i < length; i++)
                {
                    var key = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;

                    switch (key)
                    {
                        case Keys.Code:
                            code = (CommandCodes) MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                            offset += readSize;
                            break;
                        case Keys.Sync:
                            sync = new RequestId(MessagePackBinary.ReadUInt64(bytes, offset, out readSize));
                            offset += readSize;
                            break;
                        case Keys.SchemaId:
                            schemaId = MessagePackBinary.ReadUInt64(bytes, offset, out readSize);
                            offset += readSize;
                            break;
                        default:
                            readSize = MessagePackBinary.ReadNextBlock(bytes, offset);
                            offset += readSize;
                            break;
                    }
                }

                readSize = offset - startOffset;

                if (!code.HasValue)
                {
                    throw ExceptionHelper.PropertyUnspecified("Code");
                }

                if (!sync.HasValue)
                {
                    throw ExceptionHelper.PropertyUnspecified("Sync");
                }

                return new ResponseHeader(code.Value, sync.Value, schemaId);
            }
        }
    }
}