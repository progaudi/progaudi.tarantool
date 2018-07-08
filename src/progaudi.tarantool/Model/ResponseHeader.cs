using System;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model
{
    public class ResponseHeader : HeaderBase
    {
        public ResponseHeader(CommandCodes code, RequestId requestId, ulong? schemaId) : base(code, requestId)
        {
            SchemaId = schemaId;
        }

        public ulong? SchemaId { get; }

        public sealed class Formatter : IMsgPackConverter<ResponseHeader>
        {
            private IMsgPackConverter<uint> _keyConverter;
            private IMsgPackConverter<ulong> _ulongConverter;
            private IMsgPackConverter<CommandCodes> _codeConverter;

            public void Initialize(MsgPackContext context)
            {
                _keyConverter = context.GetConverter<uint>();
                _ulongConverter = context.GetConverter<ulong>();
                _codeConverter = context.GetConverter<CommandCodes>();
            }

            public void Write(ResponseHeader value, IMsgPackWriter writer)
            {
                throw new NotImplementedException();
            }

            public ResponseHeader Read(IMsgPackReader reader)
            {
                var length = reader.ReadMapLength();

                if (!length.HasValue)
                {
                    throw ExceptionHelper.InvalidMapLength(length, 2u, 3u);
                }

                CommandCodes? code = null;
                RequestId? sync = null;
                ulong? schemaId = null;

                for (int i = 0; i < length.Value; i++)
                {
                    var key = _keyConverter.Read(reader);

                    switch (key)
                    {
                        case Keys.Code:
                            code = _codeConverter.Read(reader);
                            break;
                        case Keys.Sync:
                            sync = new RequestId(_ulongConverter.Read(reader));
                            break;
                        case Keys.SchemaId:
                            schemaId = _ulongConverter.Read(reader);
                            break;
                        default:
                            reader.SkipToken();
                            break;
                    }
                }

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