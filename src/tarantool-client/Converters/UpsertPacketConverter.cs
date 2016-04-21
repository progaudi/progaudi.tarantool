using System;

using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpsertPacketConverter<T1, TUpdate> : IMsgPackConverter<UpsertPacket<T1, TUpdate>>
    {
        public void Write(UpsertPacket<T1, TUpdate> value, IBytesWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter < Tuple<T1>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, TUpdate> Read(IBytesReader reader, MsgPackContext context, Func<UpsertPacket<T1, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }
}