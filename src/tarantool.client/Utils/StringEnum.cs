using System;
using System.Reflection;

namespace Tarantool.Client.Utils
{
    internal class StringValueAttribute : Attribute
    {
        public StringValueAttribute(string value)
        {
            Value = value;
        }

        public string Value { get; }
    }

    internal class StringEnum
    {
        public static T Parse<T>(Type type, string stringValue, bool ignoreCase)
            where T : struct
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
                foreach (var fi in type.GetFields())
                {
                    //Check for our custom attribute
                    var attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
                    if (attrs.Length > 0)
                        enumStringValue = attrs[0].Value;

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