using System;
using System.Globalization;
using System.Reflection;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class EnumConverter<T> : IMsgPackConverter<T>
        where T : struct, IConvertible
    {
        private IMsgPackConverter<sbyte> _sbyteConverter;
        private IMsgPackConverter<byte> _byteConverter;
        private IMsgPackConverter<short> _shortConverter;
        private IMsgPackConverter<ushort> _ushortConverter;
        private IMsgPackConverter<int> _intConverter;
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<long> _longConverter;
        private IMsgPackConverter<ulong> _ulongConverter;

        public void Initialize(MsgPackContext context)
        {
            _sbyteConverter = context.GetConverter<sbyte>();
            _byteConverter = context.GetConverter<byte>();
            _shortConverter = context.GetConverter<short>();
            _ushortConverter = context.GetConverter<ushort>();
            _intConverter = context.GetConverter<int>();
            _uintConverter = context.GetConverter<uint>();
            _longConverter = context.GetConverter<long>();
            _ulongConverter = context.GetConverter<ulong>();
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
                _sbyteConverter.Write(value.ToSByte(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                _byteConverter.Write(value.ToByte(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                _shortConverter.Write(value.ToInt16(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                _ushortConverter.Write(value.ToUInt16(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                _intConverter.Write(value.ToInt32(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                _uintConverter.Write(value.ToUInt32(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                _longConverter.Write(value.ToInt64(CultureInfo.InvariantCulture), writer);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                _ulongConverter.Write(value.ToUInt64(CultureInfo.InvariantCulture), writer);
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
                var readValue = _sbyteConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(byte))
            {
                var readValue = _byteConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(short))
            {
                var readValue = _shortConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(ushort))
            {
                var readValue = _ushortConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(int))
            {
                var readValue = _intConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(uint))
            {
                var readValue = _uintConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(long))
            {
                var readValue = _longConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else if (enumUnderlyingType == typeof(ulong))
            {
                var readValue = _ulongConverter.Read(reader);
                return (T) Enum.ToObject(typeof(T), readValue);
            }
            else
            {
                throw new InvalidOperationException($"Unexpected underlying enum type: {enumUnderlyingType}.");
            }
        }
    }
}