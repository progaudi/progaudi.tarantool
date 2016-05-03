using System;
using System.Globalization;
using MsgPack.Light;
using System.Reflection;
using tarantool_client.Utils;

namespace tarantool_client.Converters
{
    public class FromStringEnumConverter<T> : IMsgPackConverter<T>
           where T : struct, IConvertible
    {
        static FromStringEnumConverter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw new InvalidOperationException($"Enum expected, but got {typeof(T)}.");
            }
        }

        public void Write(T value, IMsgPackWriter writer, MsgPackContext context)
        {
            var stringConverter = context.GetConverter<string>();
            stringConverter.Write(value.ToString(CultureInfo.InvariantCulture), writer, context);
        }

        public T Read(IMsgPackReader reader, MsgPackContext context, Func<T> creator)
        {
            var stringConverter = context.GetConverter<string>();

            var stringValue = stringConverter.Read(reader, context, null);

            return  StringEnum.Parse<T>(typeof (T), stringValue, true);
        }
    }
}