using System;

using iproto.Data;
using iproto.Data.Packets;

using JetBrains.Annotations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class SelectPacketConverter<T1> : IMsgPackConverter<SelectPacket<T1>>
    {
        public void Write(SelectPacket<T1> value, IBytesWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var iteratorConverter = context.GetConverter<Iterator>();
            var selectKeyConverter = context.GetConverter<Tuple<T1>>();

            writer.WriteMapHeaderAndLength(6);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            intConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Limit, writer, context);
            intConverter.Write(value.Limit, writer, context);

            keyConverter.Write(Key.Offset, writer, context);
            intConverter.Write(value.Offset, writer, context);

            keyConverter.Write(Key.Iterator, writer, context);
            iteratorConverter.Write(value.Iterator, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            writer.WriteArrayHeaderAndLength(1);
            selectKeyConverter.Write(value.SelectKey, writer, context);
        }

        public SelectPacket<T1> Read(IBytesReader reader, MsgPackContext context, Func<SelectPacket<T1>> creator)
        {
            throw new NotImplementedException();
        }
    }
}