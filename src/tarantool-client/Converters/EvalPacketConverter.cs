using System;

using iproto;
using iproto.Data;
using iproto.Data.Packets;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class EvalPacketConverter<T> : IMsgPackConverter<EvalPacket<T>>
        where T:IMyTuple
    {
        public void Write(EvalPacket<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var headerConverter = context.GetConverter<Header>();
            headerConverter.Write(value.Header, writer, context);

            var keyConverter = context.GetConverter<Key>();
            var stringConverter = context.GetConverter<string>();
            var tupleConverter = context.GetConverter<T>();

            writer.WriteMapHeaderAndLength(2);

            keyConverter.Write(Key.Expression, writer, context);
            stringConverter.Write(value.Expression, writer, context);

            keyConverter.Write(Key.Tuple, writer, context);
            tupleConverter.Write(value.Tuple, writer, context);
        }

        public EvalPacket<T> Read(IMsgPackReader reader, MsgPackContext context, Func<EvalPacket<T>> creator)
        {
            throw new NotImplementedException();
        }
    }
}