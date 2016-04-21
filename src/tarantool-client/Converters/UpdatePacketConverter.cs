using System;

using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpdatePacketConverter<T1> : IMsgPackConverter<UpdatePacket<T1>>
    {
        public void Write(UpdatePacket<T1> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1>>();
            var updateOperationConverter = context.GetConverter<IUpdateOperation>(value.UpdateOperation.GetType());

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            intConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            writer.WriteArrayHeader(1);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1> Read(IMsgPackReader reader, MsgPackContext context, Func<UpdatePacket<T1>> creator)
        {
            throw new NotImplementedException();
        }
    }
}