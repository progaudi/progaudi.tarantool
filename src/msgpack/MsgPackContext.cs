using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

using TarantoolDnx.MsgPack.Converters;

namespace TarantoolDnx.MsgPack
{
    
    public class MsgPackContext
    {
        private static readonly IReadOnlyDictionary<Type, IMsgPackConverter> DefaultConverters = new Dictionary<Type, IMsgPackConverter>
        {
            {typeof (bool), new BoolConverter()},
            {typeof (string), new StringConverter()},
            {typeof (byte[]), new BinaryConverter()},
            {typeof (float), new FloatConverter()},
            {typeof (double), new FloatConverter()},
            {typeof (byte), new IntConverter()},
            {typeof (sbyte), new IntConverter()},
            {typeof (short), new IntConverter()},
            {typeof (ushort), new IntConverter()},
            {typeof (int), new IntConverter()},
            {typeof (uint), new IntConverter()},
            {typeof (long), new IntConverter()},
            {typeof (ulong), new IntConverter()},
            {typeof (DateTime), new DateTimeConverter()},
            {typeof (DateTimeOffset), new DateTimeConverter()},

            {typeof (bool?), new NullableConverter<bool>()},
            {typeof (float?), new NullableConverter<float>()},
            {typeof (double?), new NullableConverter<double>()},
            {typeof (byte?), new NullableConverter<byte>()},
            {typeof (sbyte?), new NullableConverter<sbyte>()},
            {typeof (short?), new NullableConverter<short>()},
            {typeof (ushort?), new NullableConverter<ushort>()},
            {typeof (int?), new NullableConverter<int>()},
            {typeof (uint?), new NullableConverter<uint>()},
            {typeof (long?), new NullableConverter<long>()},
            {typeof (ulong?), new NullableConverter<ulong>()},
            {typeof (DateTime?), new NullableConverter<DateTime>()},
            {typeof (DateTimeOffset?), new NullableConverter<DateTimeOffset>()}
        };

        private static readonly ConcurrentDictionary<Type, IMsgPackConverter> GeneratedConverters = new ConcurrentDictionary<Type, IMsgPackConverter>();

        private static readonly ConcurrentDictionary<Type, Func<object>> ObjectActivators = new ConcurrentDictionary<Type, Func<object>>();

        private static readonly IMsgPackConverter<object> SharedNullConverter = new NullConverter();

        private readonly Dictionary<Type, IMsgPackConverter> _converters = new Dictionary<Type, IMsgPackConverter>();

        private readonly Dictionary<Type, Type> _genericConverters = new Dictionary<Type, Type>();

        public IMsgPackConverter<object> NullConverter => SharedNullConverter;

        public void RegisterConverter<T>(IMsgPackConverter<T> converter)
        {
            _converters[typeof(T)] = converter;
        }

        public void RegisterGenericConverter(Type type)
        {
            var converterType= GetGenericInterface(type, typeof(IMsgPackConverter<>));
            if (converterType == null)
            {
                throw new ArgumentException($"Error registering generic converter. Expected IMsgPackConverter<> implementation, but got {type}");
            }

            var convertedType = converterType.GenericTypeArguments.Single().GetGenericTypeDefinition();
            _genericConverters.Add(convertedType, type);
        }

        public IMsgPackConverter<T> GetConverter<T>()
        {
            var type = typeof(T);
            return (IMsgPackConverter<T>)
                (GetConverterFromCache(type)
                ?? TryGenerateConverterFromGenericConverter(type)
                ?? TryGenerateArrayConverter(type)
                ?? TryGenerateMapConverter(type)
                ?? TryGenerateNullableConverter(type));
        }

        private IMsgPackConverter TryGenerateConverterFromGenericConverter(Type type)
        {
            var genericType = type.GetGenericTypeDefinition();
            Type genericConverterType;

            if (!_genericConverters.TryGetValue(genericType, out genericConverterType))
            {
                return null;
            }

            var converterType = genericConverterType.MakeGenericType(type.GenericTypeArguments);
            return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
        }

        public Func<object> GetObjectActivator(Type type)
        {
            return ObjectActivators.GetOrAdd(type, t => CompiledLambdaActivatorFactory.GetActivator(type));
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
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
            }

            mapInterface = GetGenericInterface(type, typeof(IReadOnlyDictionary<,>));
            if (mapInterface != null)
            {
                var converterType = typeof(ReadOnlyMapConverter<,,>).MakeGenericType(
                    type,
                    mapInterface.GenericTypeArguments[0],
                    mapInterface.GenericTypeArguments[1]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
            }

            return null;
        }

        private IMsgPackConverter TryGenerateNullableConverter(Type type)
        {
            var typeInfo = type.GetTypeInfo();
            if (!typeInfo.IsGenericType || typeInfo.GetGenericTypeDefinition() != typeof (Nullable<>))
            {
                return null;
            }

            var converterType = typeof(NullableConverter<>).MakeGenericType(typeInfo.GenericTypeArguments[0]);
            return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
        }

        private IMsgPackConverter TryGenerateArrayConverter(Type type)
        {
            var arrayInterface = GetGenericInterface(type, typeof(IList<>));
            if (arrayInterface != null)
            {
                var converterType = typeof(ArrayConverter<,>).MakeGenericType(type, arrayInterface.GenericTypeArguments[0]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
            }

            arrayInterface = GetGenericInterface(type, typeof(IReadOnlyList<>));
            if (arrayInterface != null)
            {
                var converterType = typeof(ReadOnlyListConverter<,>).MakeGenericType(type, arrayInterface.GenericTypeArguments[0]);
                return GeneratedConverters.GetOrAdd(converterType, x => (IMsgPackConverter)GetObjectActivator(x)());
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