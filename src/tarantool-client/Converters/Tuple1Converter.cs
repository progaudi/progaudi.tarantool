using System;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class Tuple1Converter<T1> : IMsgPackConverter<Tuple<T1>>
    {
        public void Write(Tuple<T1> value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(null, writer, context);
                return;
            }

            var t1Converter = context.GetConverter<T1>();

            t1Converter.Write(value.Item1, writer, context);
        }

        public Tuple<T1> Read(IBytesReader reader, MsgPackContext context, Func<Tuple<T1>> creator)
        {
            var t1Converter = context.GetConverter<T1>();

            var item1 = t1Converter.Read(reader, context, null);

            return Tuple.Create(item1);
        }
    }
}