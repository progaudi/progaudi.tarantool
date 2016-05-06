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
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
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
            var stringConverter = _context.GetConverter<string>();
            stringConverter.Write(value.ToString(CultureInfo.InvariantCulture), writer);
        }

        public T Read(IMsgPackReader reader)
        {
            var stringConverter = _context.GetConverter<string>();

            var stringValue = stringConverter.Read(reader);

            return  StringEnum.Parse<T>(typeof (T), stringValue, true);
        }
    }
}