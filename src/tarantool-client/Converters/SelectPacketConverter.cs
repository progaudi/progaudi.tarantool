using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class SelectPacketConverter<T> : IMsgPackConverter<SelectPacket<T>>
        where T : ITuple
    {
        public void Write(SelectPacket<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var uintConverter = context.GetConverter<uint>();
            var iteratorConverter = context.GetConverter<Iterator>();
            var selectKeyConverter = context.GetConverter<T>();

            writer.WriteMapHeaderAndLength(6);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Limit, writer, context);
            uintConverter.Write(value.Limit, writer, context);

            keyConverter.Write(Key.Offset, writer, context);
            uintConverter.Write(value.Offset, writer, context);

            keyConverter.Write(Key.Iterator, writer, context);
            iteratorConverter.Write(value.Iterator, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.SelectKey, writer, context);
        }

        public SelectPacket<T> Read(IMsgPackReader reader, MsgPackContext context, Func<SelectPacket<T>> creator)
        {
            throw new NotImplementedException();
        }
    }
}