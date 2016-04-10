using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace TarantoolDnx.MsgPack
{
    [DebuggerStepThrough]
    public class MsgPackSettings
    {
        private static readonly IReadOnlyDictionary<Type, IMsgPackConverter> DefaultConverters = new Dictionary<Type, IMsgPackConverter>
        {
            { typeof(bool), new BoolConverter() },
            { typeof(string), new StringConverter() },
            { typeof(byte[]), new BinaryConverter() },

            { typeof(float), new FloatConverter() },
            { typeof(double), new FloatConverter() },

            { typeof(byte), new IntConverter() },
            { typeof(sbyte), new IntConverter() },
            { typeof(short), new IntConverter() },
            { typeof(ushort), new IntConverter() },
            { typeof(int), new IntConverter() },
            { typeof(uint), new IntConverter() },
            { typeof(long), new IntConverter() },
            { typeof(ulong), new IntConverter() },
        };

        private static readonly IMsgPackConverter<object> nullConverter = new NullConverter();

        private readonly Dictionary<Type, IMsgPackConverter> converters = new Dictionary<Type, IMsgPackConverter>();

        public IMsgPackConverter<object> NullConverter => nullConverter;

        public void RegisterConverter<T>(IMsgPackConverter<T> converter)
        {
            converters[typeof(T)] = converter;
        }

        public IMsgPackConverter<T> GetConverter<T>()
        {
            IMsgPackConverter temp;

            if (!converters.TryGetValue(typeof(T), out temp))
            {
                if (DefaultConverters.TryGetValue(typeof(T), out temp))
                {
                    return (IMsgPackConverter<T>)temp;
                }

                return null;
            }

            var result = temp as IMsgPackConverter<T>;
            if (result == null)
            {
                converters.Remove(typeof(T));
            }

            return result;
        }
    }
}