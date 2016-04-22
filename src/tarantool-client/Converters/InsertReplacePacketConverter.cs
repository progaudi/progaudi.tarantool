using System;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class InsertReplacePacketConverter<T1> : IMsgPackConverter<InsertReplacePacket<T1>>
    {
        public void Write(InsertReplacePacket<T1> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1> Read(IMsgPackReader reader, MsgPackContext context, Func<InsertReplacePacket<T1>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2> : IMsgPackConverter<InsertReplacePacket<T1, T2>>
    {
        public void Write(InsertReplacePacket<T1, T2> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2> Read(IMsgPackReader reader, MsgPackContext context, Func<InsertReplacePacket<T1, T2>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2, T3> : IMsgPackConverter<InsertReplacePacket<T1, T2, T3>>
    {
        public void Write(InsertReplacePacket<T1, T2, T3> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2, T3> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<InsertReplacePacket<T1, T2, T3>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2, T3, T4> : IMsgPackConverter<InsertReplacePacket<T1, T2, T3, T4>>
    {
        public void Write(InsertReplacePacket<T1, T2, T3, T4> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2, T3, T4> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<InsertReplacePacket<T1, T2, T3, T4>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2, T3, T4, T5> : IMsgPackConverter<InsertReplacePacket<T1, T2, T3, T4, T5>>
    {
        public void Write(InsertReplacePacket<T1, T2, T3, T4, T5> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2, T3, T4, T5> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<InsertReplacePacket<T1, T2, T3, T4, T5>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2, T3, T4, T5, T6> : IMsgPackConverter<InsertReplacePacket<T1, T2, T3, T4, T5, T6>>
    {
        public void Write(InsertReplacePacket<T1, T2, T3, T4, T5, T6> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2, T3, T4, T5, T6> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<InsertReplacePacket<T1, T2, T3, T4, T5, T6>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2, T3, T4, T5, T6, T7> :
        IMsgPackConverter<InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7>>
    {
        public void Write(InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6, T7>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7>> creator)
        {
            throw new NotImplementedException();
        }
    }

    public class InsertReplacePacketConverter<T1, T2, T3, T4, T5, T6, T7, TRest> :
        IMsgPackConverter<InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7, TRest>>
    {
        public void Write(InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7, TRest> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var intConverter = context.GetConverter<int>();
            var tupleConverter = context.GetConverter<Tuple<T1, T2, T3, T4, T5, T6, T7, TRest>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.SpaceId, writer, context);
            intConverter.Write(value.SpaceId, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7, TRest> Read(
            IMsgPackReader reader,
            MsgPackContext context,
            Func<InsertReplacePacket<T1, T2, T3, T4, T5, T6, T7, TRest>> creator)
        {
            throw new NotImplementedException();
        }
    }
}