using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpsertPacketConverter<T, TUpdate> : IMsgPackConverter<UpsertPacket<T, TUpdate>>
        where T : ITuple
    {
        public void Write(UpsertPacket<T, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<T>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T, TUpdate> Read(IMsgPackReader reader, MsgPackContext context, Func<UpsertPacket<T, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }
}