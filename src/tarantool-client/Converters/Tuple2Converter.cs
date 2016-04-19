using System;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class Tuple2Converter <T1,T2>: IMsgPackConverter<Tuple<T1,T2>>
    {
        public void Write(Tuple<T1, T2> value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(null, writer, context);
            }
            else
            {
                var t1Converter = context.GetConverter<T1>();
                var t2Converter = context.GetConverter<T2>();

                t1Converter.Write(value.Item1, writer, context);
                t2Converter.Write(value.Item2, writer, context);
            }
        }

        public Tuple<T1, T2> Read(IBytesReader reader, MsgPackContext context, Func<Tuple<T1, T2>> creator)
        {
            var t1Converter = context.GetConverter<T1>();
            var t2Converter = context.GetConverter<T2>();

            var item1 = t1Converter.Read(reader, context, null);
            var item2 = t2Converter.Read(reader, context, null);

            return Tuple.Create(item1, item2);
        }
    }
}