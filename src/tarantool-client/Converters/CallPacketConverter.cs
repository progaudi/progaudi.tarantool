using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class CallPacketConverter<T> : IMsgPackConverter<CallPacket<T>>
        where T : IMyTuple
    {
        public void Write(CallPacket<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var stringConverter = context.GetConverter<string>();
            var tupleConverter = context.GetConverter<T>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.FunctionName, writer, context);
            stringConverter.Write(value.FunctionName, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeader(1);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public CallPacket<T> Read(IMsgPackReader reader, MsgPackContext context, Func<CallPacket<T>> creator)
        {
            throw new NotImplementedException();
        }
    }
}