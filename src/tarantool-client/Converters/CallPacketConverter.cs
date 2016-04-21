using System;

using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class CallPacketConverter<T1> : IMsgPackConverter<CallPacket<T1>>
    {
        public void Write(CallPacket<T1> value, IBytesWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var stringConverter = context.GetConverter<string>();
            var tupleConverter = context.GetConverter<Tuple<T1>>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.FunctionName, writer, context);
            stringConverter.Write(value.FunctionName, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            writer.WriteArrayHeaderAndLength(1);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public CallPacket<T1> Read(IBytesReader reader, MsgPackContext context, Func<CallPacket<T1>> creator)
        {
            throw new NotImplementedException();
        }
    }
}