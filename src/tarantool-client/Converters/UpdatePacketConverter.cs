using System;

using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpdatePacketConverter<T1, TUpdate> : IMsgPackConverter<UpdatePacket<T1, TUpdate>>
    {
        public void Write(UpdatePacket<T1, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(1u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, TUpdate> Read(IMsgPackReader reader, MsgPackContext context, Func<UpdatePacket<T1, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, TUpdate> : IMsgPackConverter<UpdatePacket<T1, T2, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(2u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, T3, TUpdate> : IMsgPackConverter<UpdatePacket<T1, T2, T3, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, T3, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2, T3>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(3u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, T3, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, T3, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, T3, T4, TUpdate> : IMsgPackConverter<UpdatePacket<T1, T2, T3, T4, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, T3, T4, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2, T3, T4>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(4u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, T3, T4, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, T3, T4, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, T3, T4, T5, TUpdate> : IMsgPackConverter<UpdatePacket<T1, T2, T3, T4, T5, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, T3, T4, T5, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(5u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, T3, T4, T5, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, T3, T4, T5, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, T3, T4, T5, T6, TUpdate> : IMsgPackConverter<UpdatePacket<T1, T2, T3, T4, T5, T6, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, T3, T4, T5, T6, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(6u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, T3, T4, T5, T6, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, T3, T4, T5, T6, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, T3, T4, T5, T6, T7, TUpdate> :
        IMsgPackConverter<UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6, T7>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(7u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpdatePacketConverter<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> :
        IMsgPackConverter<UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate>>
    {
        public void Write(UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var uintConverter = context.GetConverter<uint>();
            var keyConverter = context.GetConverter<Key>();
            var selectKeyConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(4);

            keyConverter.Write(Key.SpaceId, writer, context);
            uintConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.IndexId, writer, context);
            uintConverter.Write(value.IndexId, writer, context);

            keyConverter.Write(Key.Key, writer, context);
            selectKeyConverter.Write(value.Key, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(8u);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpdatePacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }
}