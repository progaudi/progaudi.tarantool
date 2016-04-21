using System;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class InsertReplacePacketConverter<T1> : IMsgPackConverter<InsertReplacePacket<T1>>
    {
        public void Write(InsertReplacePacket<T1> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1> Read(IMsgPackReader reader, MsgPackContext context, Func<InsertReplacePacket<T1>> creator)
        {
            throw new NotImplementedException();
        }
    }
}