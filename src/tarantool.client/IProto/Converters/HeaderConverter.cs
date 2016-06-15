using MsgPack.Light;

using Shouldly;

using Tarantool.Client.IProto.Data;

namespace Tarantool.Client.IProto.Converters
{
    public class HeaderConverter : IMsgPackConverter<Header>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<ulong> _ulongConverter;
        private IMsgPackConverter<CommandCode> _codeConverter;
        private IMsgPackConverter<object> _nullConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _ulongConverter = context.GetConverter<ulong>();
            _codeConverter = context.GetConverter<CommandCode>();
            _nullConverter = context.NullConverter;
        }

        public void Write(Header value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _nullConverter.Write(null, writer);
                return;
            }

            
            var headerComponentsCount = 1u;
            if (value.Sync.HasValue)
            {
                headerComponentsCount ++;
            }
            if (value.SchemaId.HasValue)
            {
                headerComponentsCount++;
            }
            writer.WriteMapHeader(headerComponentsCount);

            _keyConverter.Write(Key.Code, writer);
            _codeConverter.Write(value.Code, writer);

            if (value.Sync.HasValue)
            {
                _keyConverter.Write(Key.Sync, writer);
                _ulongConverter.Write(value.Sync.Value, writer);
            }

            if (value.SchemaId.HasValue)
            {
                _keyConverter.Write(Key.SchemaId, writer);
                _ulongConverter.Write(value.SchemaId.Value, writer);
            }
        }

        public Header Read(IMsgPackReader reader)
        {
            reader.ReadMapLength().ShouldBe(3u);

            _keyConverter.Read(reader).ShouldBe(Key.Code);
            var code = _codeConverter.Read(reader);

            _keyConverter.Read(reader).ShouldBe(Key.Sync);
            var sync = _ulongConverter.Read(reader);

            _keyConverter.Read(reader).ShouldBe(Key.SchemaId);
            var schemaId = _ulongConverter.Read(reader);

            return new Header(code, sync, schemaId);
        }
    }
}