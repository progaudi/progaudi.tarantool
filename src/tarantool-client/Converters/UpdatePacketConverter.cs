using System;

using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpdatePacketConverter<T1, TUpdate> : IMsgPackConverter<UpdatePacket<T1, TUpdate>>
    {
        public void Write(UpdatePacket<T1, TUpdate> value, IBytesWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            intConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            writer.WriteArrayHeaderAndLength(1);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, TUpdate> Read(IBytesReader reader, MsgPackContext context, Func<UpdatePacket<T1, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }
}