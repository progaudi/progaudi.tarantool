using System;

using iproto.Data;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class KeyConverter :IMsgPackConverter<Key>
    {
        public void Write(Key value, IMsgPackWriter writer, MsgPackContext context)
        {
            var intConverter = context.GetConverter<int>();
            intConverter.Write((int) value, writer, context);
        }

        public Key Read(IMsgPackReader reader, MsgPackContext context, Func<Key> creator)
        {
            var intConverter = context.GetConverter<int>();
            return (Key)intConverter.Read(reader, context, null);
        }
    }
}