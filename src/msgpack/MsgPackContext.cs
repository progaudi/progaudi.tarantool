using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace TarantoolDnx.MsgPack
{
    [DebuggerStepThrough]
    public class MsgPackContext
    {
        private static readonly IReadOnlyDictionary<Type, IMsgPackConverter> DefaultConverters = new Dictionary<Type, IMsgPackConverter>
        {
            {typeof(bool), new BoolConverter()},
            {typeof(string), new StringConverter()},
            {typeof(byte[]), new BinaryConverter()},
            {typeof(float), new FloatConverter()},
            {typeof(double), new FloatConverter()},
            {typeof(byte), new IntConverter()},
            {typeof(sbyte), new IntConverter()},
            {typeof(short), new IntConverter()},
            {typeof(ushort), new IntConverter()},
            {typeof(int), new IntConverter()},
            {typeof(uint), new IntConverter()},
            {typeof(long), new IntConverter()},
            {typeof(ulong), new IntConverter()}
        };

        private static readonly ConcurrentDictionary<Type, IMsgPackConverter> GeneratedConverters = new ConcurrentDictionary<Type, IMsgPackConverter>();

        private static readonly IMsgPackConverter<object> SharedNullConverter = new NullConverter();

        private readonly Dictionary<Type, IMsgPackConverter> _converters = new Dictionary<Type, IMsgPackConverter>();

        public IMsgPackConverter<object> NullConverter => SharedNullConverter;

        public void RegisterConverter<T>(IMsgPackConverter<T> converter)
        {
            _converters[typeof(T)] = converter;
        }

        public IMsgPackConverter<T> GetConverter<T>()
        {
            var type = typeof(T);
            return (IMsgPackConverter<T>) (GetConverterFromCache(type)
                ?? TryGenerateArrayConverter(type)
                ?? TryGenerateMapConverter(type));
        }

        private IMsgPackConverter TryGenerateMapConverter(Type type)
        {
            var mapInterface = GetGenericInterface(type, typeof(IDictionary<,>));
            if (mapInterface != null)
            {
                var converterType = typeof(MapConverter<,,>).MakeGenericType(
                    type,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter) Activator.CreateInstance(converterType));
            }

            mapInterface = GetGenericInterface(type, typeof(IReadOnlyDictionary<,>));
            if (mapInterface != null)
            {
                var converterType = typeof(ReadOnlyMapConverter<,,>).MakeGenericType(
                    type,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter) Activator.CreateInstance(converterType));
            }

            return null;
        }

        private IMsgPackConverter TryGenerateArrayConverter(Type type)
        {
            var arrayInterface = GetGenericInterface(type, typeof(IList<>));
            if (arrayInterface != null)
            {
                var converterType = typeof(ArrayConverter<,>).MakeGenericType(type, arrayInterface.GenericTypeArguments[0]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter) Activator.CreateInstance(converterType));
            }

            arrayInterface = GetGenericInterface(type, typeof(IReadOnlyList<>));
            if (arrayInterface != null)
            {
                var converterType = typeof(ReadOnlyListConverter<,>).MakeGenericType(type, arrayInterface.GenericTypeArguments[0]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter) Activator.CreateInstance(converterType));
            }

            return null;
        }

        private IMsgPackConverter GetConverterFromCache(Type type)
        {
            IMsgPackConverter temp;

            if (_converters.TryGetValue(type, out temp))
                return temp;

            if (DefaultConverters.TryGetValue(type, out temp))
                return temp;

            return null;
        }

        private static TypeInfo GetGenericInterface(Type type, Type genericInterfaceType)
        {
            var info = type.GetTypeInfo();
            if (info.IsInterface && info.IsGenericType && info.GetGenericTypeDefinition() == genericInterfaceType)
            {
                return info;
            }

            return info
                .ImplementedInterfaces
                .Select(x => x.GetTypeInfo())
                .FirstOrDefault(x => x.IsGenericType && x.GetGenericTypeDefinition() == genericInterfaceType);
        }
    }
}