using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class DeletePacketConverter<T> : IMsgPackConverter<DeletePacket<T>>
        where T: ITuple
    {
        public void Write(DeletePacket<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var uintConverter = context.GetConverter<uint>();
            var selectKeyConverter = context.GetConverter<T>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);
        }

        public DeletePacket<T> Read(IMsgPackReader reader, MsgPackContext context, Func<DeletePacket<T>> creator)
        {
            throw new NotImplementedException();
        }
    }
}