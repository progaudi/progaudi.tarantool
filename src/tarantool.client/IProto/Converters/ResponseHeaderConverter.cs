using System;

using MsgPack.Light;

using Shouldly;

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
            reader.ReadMapLength().ShouldBe(3u);

            _keyConverter.Read(reader).ShouldBe(Key.Code);
            var code = _codeConverter.Read(reader);

            _keyConverter.Read(reader).ShouldBe(Key.Sync);
            var sync = _requestIdConverter.Read(reader);

            _keyConverter.Read(reader).ShouldBe(Key.SchemaId);
            var schemaId = _ulongConverter.Read(reader);

            return new ResponseHeader(code, sync, schemaId);
        }
    }
}