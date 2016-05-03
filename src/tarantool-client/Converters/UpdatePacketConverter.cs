using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class UpdatePacketConverter<T, TUpdate> : IMsgPackConverter<UpdatePacket<T, TUpdate>>
        where T : ITuple
    {
        public void Write(UpdatePacket<T, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<T>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(1);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T, TUpdate> Read(IMsgPackReader reader, MsgPackContext context, Func<UpdatePacket<T, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }
}