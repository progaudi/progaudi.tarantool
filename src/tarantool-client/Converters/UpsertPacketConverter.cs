using System;

using iproto.Data;
using iproto.Data.Packets;
using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpsertPacketConverter<T1, TUpdate> : IMsgPackConverter<UpsertPacket<T1, TUpdate>>
    {
        public void Write(UpsertPacket<T1, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, TUpdate> Read(IMsgPackReader reader, MsgPackContext context, Func<UpsertPacket<T1, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, TUpdate> : IMsgPackConverter<UpsertPacket<T1, T2, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, T3, TUpdate> : IMsgPackConverter<UpsertPacket<T1, T2, T3, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, T3, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, T3, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, T3, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, T3, T4, TUpdate> : IMsgPackConverter<UpsertPacket<T1, T2, T3, T4, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, T3, T4, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, T3, T4, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, T3, T4, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, T3, T4, T5, TUpdate> : IMsgPackConverter<UpsertPacket<T1, T2, T3, T4, T5, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, T3, T4, T5, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, T3, T4, T5, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, T3, T4, T5, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, T3, T4, T5, T6, TUpdate> : IMsgPackConverter<UpsertPacket<T1, T2, T3, T4, T5, T6, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, T3, T4, T5, T6, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, T3, T4, T5, T6, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, T3, T4, T5, T6, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, T3, T4, T5, T6, T7, TUpdate> :
        IMsgPackConverter<UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6, T7>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class UpsertPacketConverter<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> :
        IMsgPackConverter<UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate>>
    {
        public void Write(UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var intConverter = context.GetConverter<int>();
            var keyConverter = context.GetConverter<Key>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>();
            var updateOperationConverter = context.GetConverter<UpdateOperation<TUpdate>>();

            writer.WriteMapHeaderAndLength(3);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);

            keyConverter.Write(Key.Ops, writer, context);
            updateOperationConverter.Write(value.UpdateOperation, writer, context);
        }

        public UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<UpsertPacket<T1, T2, T3, T4, T5, T6, T7, TRest, TUpdate>> creator)
        {
            throw new NotImplementedException();
        }
    }
}