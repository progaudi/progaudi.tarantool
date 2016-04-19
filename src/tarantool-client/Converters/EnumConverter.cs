using System;
using System.Globalization;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class EnumConverter<T> : IMsgPackConverter<T>
        where T : struct, IConvertible
    {
        public void Write(T value, IBytesWriter writer, MsgPackContext context)
        {
            var intConverter = context.GetConverter<int>();
            intConverter.Write(value.ToInt32(CultureInfo.InvariantCulture), writer, context);
        }

        public T Read(IBytesReader reader, MsgPackContext context, Func<T> creator)
        {
            var intConverter = context.GetConverter<int>();
            return (T)(object)intConverter.Read(reader, context, null);
        }
    }
}