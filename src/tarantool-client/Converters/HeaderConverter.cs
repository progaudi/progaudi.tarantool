using iproto.Data;

using MsgPack.Light;

using Shouldly;

namespace tarantool_client.Converters
{
    public class HeaderConverter : IMsgPackConverter<Header>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(Header value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _context.NullConverter.Write(null, writer);
                return;
            }

            var keyConverter = _context.GetConverter<Key>();
            var ulongConverter = _context.GetConverter<ulong>();
            var codeConverter = _context.GetConverter<CommandCode>();

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

            keyConverter.Write(Key.Code, writer);
            codeConverter.Write(value.Code, writer);

            if (value.Sync.HasValue)
            {
                keyConverter.Write(Key.Sync, writer);
                ulongConverter.Write(value.Sync.Value, writer);
            }

            if (value.SchemaId.HasValue)
            {
                keyConverter.Write(Key.SchemaId, writer);
                ulongConverter.Write(value.SchemaId.Value, writer);
            }
        }

        public Header Read(IMsgPackReader reader)
        {
            var keyConverter = _context.GetConverter<Key>();
            var ulongConverter = _context.GetConverter<ulong>();
            var codeConverter = _context.GetConverter<CommandCode>();

            reader.ReadMapLength().ShouldBe(3u);

            keyConverter.Read(reader).ShouldBe(Key.Code);
            var code = codeConverter.Read(reader);

            keyConverter.Read(reader).ShouldBe(Key.Sync);
            var sync = ulongConverter.Read(reader);

            keyConverter.Read(reader).ShouldBe(Key.SchemaId);
            var schemaId = ulongConverter.Read(reader);

            return new Header(code, sync, schemaId);
        }
    }
}