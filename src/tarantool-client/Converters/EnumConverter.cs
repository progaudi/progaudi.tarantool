using System;
using System.Globalization;
using System.Reflection;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class EnumConverter<T> : IMsgPackConverter<T>
        where T : struct, IConvertible
    {
        static EnumConverter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw new InvalidOperationException($"Enum expected, but got {typeof(T)}.");
            }
        }

        public void Write(T value, IMsgPackWriter writer, MsgPackContext context)
        {
            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));

            if (enumUnderlyingType == typeof(sbyte))
            {
                var converter = context.GetConverter<sbyte>();
                converter.Write(value.ToSByte(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                var converter = context.GetConverter<byte>();
                converter.Write(value.ToByte(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                var converter = context.GetConverter<short>();
                converter.Write(value.ToInt16(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                var converter = context.GetConverter<ushort>();
                converter.Write(value.ToUInt16(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                var converter = context.GetConverter<int>();
                converter.Write(value.ToInt32(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                var converter = context.GetConverter<uint>();
                converter.Write(value.ToUInt32(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                var converter = context.GetConverter<long>();
                converter.Write(value.ToInt64(CultureInfo.InvariantCulture), writer, context);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                var converter = context.GetConverter<ulong>();
                converter.Write(value.ToUInt64(CultureInfo.InvariantCulture), writer, context);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
            }
        }

        public T Read(IMsgPackReader reader, MsgPackContext context, Func<T> creator)
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw new InvalidOperationException($"Enum expected, but got {typeof(T)}.");
            }

            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));

            if (enumUnderlyingType == typeof(sbyte))
            {
                var converter = context.GetConverter<sbyte>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                var converter = context.GetConverter<byte>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                var converter = context.GetConverter<short>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                var converter = context.GetConverter<ushort>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                var converter = context.GetConverter<int>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                var converter = context.GetConverter<uint>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                var converter = context.GetConverter<long>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                var converter = context.GetConverter<ulong>();
                var readValue = converter.Read(reader, context, null);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
            }
        }
    }
}