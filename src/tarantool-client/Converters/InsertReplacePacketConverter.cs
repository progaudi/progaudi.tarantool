using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class InsertReplacePacketConverter<T> : IMsgPackConverter<InsertReplacePacket<T>>
        where T : ITuple
    {
        public void Write(InsertReplacePacket<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var uintConverter = context.GetConverter<uint>();
            var tupleConverter = context.GetConverter<T>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T> Read(IMsgPackReader reader, MsgPackContext context, Func<InsertReplacePacket<T>> creator)
        {
            throw new NotImplementedException();
        }
    }
}