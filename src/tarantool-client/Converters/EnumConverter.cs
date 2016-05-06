using System;
using System.Globalization;
using System.Reflection;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class EnumConverter<T> : IMsgPackConverter<T>
        where T : struct, IConvertible
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        static EnumConverter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw new InvalidOperationException($"Enum expected, but got {typeof(T)}.");
            }
        }

        public void Write(T value, IMsgPackWriter writer)
        {
            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));

            if (enumUnderlyingType == typeof(sbyte))
            {
                var converter = _context.GetConverter<sbyte>();
                converter.Write(value.ToSByte(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                var converter = _context.GetConverter<byte>();
                converter.Write(value.ToByte(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                var converter = _context.GetConverter<short>();
                converter.Write(value.ToInt16(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                var converter = _context.GetConverter<ushort>();
                converter.Write(value.ToUInt16(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                var converter = _context.GetConverter<int>();
                converter.Write(value.ToInt32(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                var converter = _context.GetConverter<uint>();
                converter.Write(value.ToUInt32(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                var converter = _context.GetConverter<long>();
                converter.Write(value.ToInt64(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                var converter = _context.GetConverter<ulong>();
                converter.Write(value.ToUInt64(CultureInfo.InvariantCulture), writer);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
            }
        }

        public T Read(IMsgPackReader reader)
        {
            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));

            if (enumUnderlyingType == typeof(sbyte))
            {
                var converter = _context.GetConverter<sbyte>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                var converter = _context.GetConverter<byte>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                var converter = _context.GetConverter<short>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                var converter = _context.GetConverter<ushort>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                var converter = _context.GetConverter<int>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                var converter = _context.GetConverter<uint>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                var converter = _context.GetConverter<long>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                var converter = _context.GetConverter<ulong>();
                var readValue = converter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
            }
        }
    }
}