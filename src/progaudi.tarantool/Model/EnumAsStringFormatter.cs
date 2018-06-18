using System;
using System.Reflection;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model
{
    public sealed class EnumAsStringFormatter<T> : IMsgPackConverter<T>
        where T : struct
    {
        private IMsgPackConverter<string> _stringConverter;

        public void Initialize(MsgPackContext context)
        {
            _stringConverter = context.GetConverter<string>();
        }

        static EnumAsStringFormatter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw ExceptionHelper.EnumExpected(enumTypeInfo);
            }
        }

        public void Write(T value, IMsgPackWriter writer)
        {
            _stringConverter.Write(value.ToString(), writer);
        }

        public T Read(IMsgPackReader reader)
        {
            var stringValue = _stringConverter.Read(reader);

            return Parse(typeof (T), stringValue, true);
        }

        public static T Parse(Type type, string stringValue, bool ignoreCase)
        {
            T output;
            string enumStringValue = null;

            if (!type.GetTypeInfo().IsEnum)
            {
                throw ExceptionHelper.EnumExpected(type);
            }

            if (!Enum.TryParse(stringValue, ignoreCase, out output))
            {

                //Look for our string value associated with fields in this enum
                foreach (var fi in type.GetRuntimeFields())
                {
                    //Check for equality then select actual enum value.
                    if (string.Compare(enumStringValue, stringValue, ignoreCase) == 0)
                    {
                        output = (T)Enum.Parse(type, fi.Name);
                        break;
                    }
                }
            }

            return output;
        }
    }
}