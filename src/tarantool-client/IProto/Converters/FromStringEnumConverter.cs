using System;
using System.Globalization;
using System.Reflection;

using MsgPack.Light;

using tarantool_client.Utils;

namespace tarantool_client.IProto.Converters
{
    public class FromStringEnumConverter<T> : IMsgPackConverter<T>
           where T : struct, IConvertible
    {
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _stringConverter = context.GetConverter<string>();
        }

        static FromStringEnumConverter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw new InvalidOperationException($"Enum expected, but got {typeof(T)}.");
            }
        }

        public void Write(T value, IMsgPackWriter writer)
        {
            _stringConverter.Write(value.ToString(CultureInfo.InvariantCulture), writer);
        }

        public T Read(IMsgPackReader reader)
        {
            var stringValue = _stringConverter.Read(reader);

            return  StringEnum.Parse<T>(typeof (T), stringValue, true);
        }
    }
}