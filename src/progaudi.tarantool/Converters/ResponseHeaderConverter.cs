using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class ResponseHeaderConverter : IMsgPackParser<ResponseHeader>
    {
        private readonly IMsgPackParser<Key> _keyConverter;
        private readonly IMsgPackParser<CommandCode> _codeConverter;

        public ResponseHeaderConverter(MsgPackContext context)
        {
            _keyConverter = context.GetRequiredParser<Key>();
            _codeConverter = context.GetRequiredParser<CommandCode>();
        }

        public ResponseHeader Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);
            
            if (!(2 <= length && length <= 3)) throw ExceptionHelper.InvalidMapLength(length, 2u, 3u);
            
            CommandCode? code = null;
            RequestId? sync = null;
            ulong? schemaId = null;

            for (var i = 0; i < length; i++)
            {
                var key = _keyConverter.Parse(source.Slice(readSize), out var temp);
                readSize += temp;

                switch (key)
                {
                    case Key.Code:
                        code = _codeConverter.Parse(source.Slice(readSize), out temp);
                        readSize += temp;
                        break;
                    case Key.Sync:
                        sync = new RequestId(MsgPackSpec.ReadUInt64(source.Slice(readSize), out temp));
                        readSize += temp;
                        break;
                    case Key.SchemaId:
                        schemaId = MsgPackSpec.ReadUInt64(source.Slice(readSize), out temp);
                        readSize += temp;
                        break;
                    default:
                        readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).Length;
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