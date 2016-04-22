using System;
using System.Runtime.Serialization;

using iproto.Data;

using TarantoolDnx.MsgPack;

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

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.Code, writer, context);
            codeConverter.Write(value.Code, writer, context);

            keyConverter.Write(Key.Sync, writer, context);
            ulongConverter.Write(value.Sync, writer, context);

            keyConverter.Write(Key.SchemaId, writer, context);
            ulongConverter.Write(value.SchemaId, writer, context);
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