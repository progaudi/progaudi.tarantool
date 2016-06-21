using System;

using MsgPack.Light;

using Tarantool.Client.IProto.Data;

namespace Tarantool.Client.IProto.Converters
{
    public class ResponseHeaderConverter : IMsgPackConverter<ResponseHeader>
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
                throw ExceptionHelper.InvalidMapLength(3u, length);
            }

            ReadKey(reader, Key.Code);
            var code = _codeConverter.Read(reader);

            ReadKey(reader, Key.Sync);
            var sync = _requestIdConverter.Read(reader);

            ReadKey(reader, Key.SchemaId);
            var schemaId = _ulongConverter.Read(reader);

            return new ResponseHeader(code, sync, schemaId);
        }

        private void ReadKey(IMsgPackReader reader, Key expected)
        {
            var actual = _keyConverter.Read(reader);
            if (actual != expected)
            {
                throw ExceptionHelper.UnexpectedKey(expected, actual);
            }
        }
    }
}