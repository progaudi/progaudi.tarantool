using System;

using iproto.Data;

using MsgPack.Light;

using Shouldly;

namespace tarantool_client.Converters
{
    public class HeaderConverter : IMsgPackConverter<Header>
    {
        public void Write(Header value, IMsgPackWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(null, writer, context);
                return;
            }

            var keyConverter = context.GetConverter<Key>();
            var ulongConverter = context.GetConverter<ulong>();
            var codeConverter = context.GetConverter<CommandCode>();

            var headerComponentsCount = 1u;
            if (value.Sync.HasValue)
            {
                headerComponentsCount ++;
            }
            if (value.SchemaId.HasValue)
            {
                headerComponentsCount++;
            }
            writer.WriteMapHeaderAndLength(headerComponentsCount);

            keyConverter.Write(Key.Code, writer, context);
            codeConverter.Write(value.Code, writer, context);

            if (value.Sync.HasValue)
            {
                keyConverter.Write(Key.Sync, writer, context);
                ulongConverter.Write(value.Sync.Value, writer, context);
            }

            if (value.SchemaId.HasValue)
            {
                keyConverter.Write(Key.SchemaId, writer, context);
                ulongConverter.Write(value.SchemaId.Value, writer, context);
            }
        }

        public Header Read(IMsgPackReader reader, MsgPackContext context, Func<Header> creator)
        {
            var keyConverter = context.GetConverter<Key>();
            var ulongConverter = context.GetConverter<ulong>();
            var codeConverter = context.GetConverter<CommandCode>();

            reader.ReadMapLength().ShouldBe(3u);

            keyConverter.Read(reader, context, null).ShouldBe(Key.Code);
            var code = codeConverter.Read(reader, context, null);

            keyConverter.Read(reader, context, null).ShouldBe(Key.Sync);
            var sync = ulongConverter.Read(reader, context, null);

            keyConverter.Read(reader, context, null).ShouldBe(Key.SchemaId);
            var schemaId = ulongConverter.Read(reader, context, null);

            return new Header(code, sync, schemaId);
        }
    }
}