using System;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.Headers;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    internal class ResponseHeaderConverter : IMsgPackConverter<ResponseHeader>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<ulong> _ulongConverter;
        private IMsgPackConverter<RequestId> _requestIdConverter;
        private IMsgPackConverter<CommandCode> _codeConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _ulongConverter = context.GetConverter<ulong>();
            _codeConverter = context.GetConverter<CommandCode>();
            _requestIdConverter = context.GetConverter<RequestId>();
        }

        public void Write(ResponseHeader value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public ResponseHeader Read(IMsgPackReader reader)
        {
            var length = reader.ReadMapLength();
            if (length != 3u)
            {
                throw ExceptionHelper.InvalidMapLength(length, 3u);
            }

            CommandCode? code = null;
            RequestId? sync = null;
            ulong? schemaId = null;

            for (int i = 0; i < length.Value; i++)
            {
                var key = _keyConverter.Read(reader);

                switch (key)
                {
                    case Key.Code:
                        code = _codeConverter.Read(reader);
                        break;
                    case Key.Sync:
                        sync = _requestIdConverter.Read(reader);
                        break;
                    case Key.SchemaId:
                        schemaId = _ulongConverter.Read(reader);
                        break;
                    default:
                        throw ExceptionHelper.UnexpectedKey(key, Key.Code, Key.Sync, Key.SchemaId);
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

            if (!schemaId.HasValue)
            {
                throw ExceptionHelper.PropertyUnspecified("SchemaId");
            }

            return new ResponseHeader(code.Value, sync.Value, schemaId.Value);
        }
    }
}