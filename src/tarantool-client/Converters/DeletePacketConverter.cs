using System;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class DeletePacketConverter<T1> : IMsgPackConverter<DeletePacket<T1>>
    {
        public void Write(DeletePacket<T1> value, IBytesWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var selectKeyConverter = context.GetConverter<Tuple<T1>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            intConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeaderAndLength(1);
            selectKeyConverter.Write(value.Key, writer, context);
        }

        public DeletePacket<T1> Read(IBytesReader reader, MsgPackContext context, Func<DeletePacket<T1>> creator)
        {
            throw new NotImplementedException();
        }
    }
}