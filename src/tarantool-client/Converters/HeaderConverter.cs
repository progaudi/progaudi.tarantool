using System;
using System.Collections.Generic;

using iproto.Data;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class HeaderConverter : IMsgPackConverter<Header>
    {
        public void Write(Header value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(null, writer, context);
                return;
            }

            var headerMapConverter = context.GetConverter<Dictionary<Key, ulong>>();
            var headerMap = new Dictionary<Key, ulong>
            {
                {Key.Code, (ulong)value.Code},
                {Key.Sync, value.Sync},
                {Key.SchemaId, value.SchemaId}
            };

            headerMapConverter.Write(headerMap, writer, context);
        }

        public Header Read(IBytesReader reader, MsgPackContext context, Func<Header> creator)
        {
            var headerMapConverter = context.GetConverter<Dictionary<Key, ulong>>();
            var headerMap = headerMapConverter.Read(reader, context, null);


            return new Header((CommandCode)headerMap[Key.Code], headerMap[Key.Sync], headerMap[Key.SchemaId]);
        }
    }
}