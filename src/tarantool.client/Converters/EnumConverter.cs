using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;

using MsgPack.Light;

using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    internal class EnumConverter<T> : IMsgPackConverter<T>
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

        private readonly Dictionary<Type, Action<T, IMsgPackWriter>> _writeMethodsCache = new Dictionary<Type, Action<T, IMsgPackWriter>>();
        private readonly Dictionary<Type, Func<IMsgPackReader, T>> _readMethodsCache = new Dictionary<Type, Func<IMsgPackReader, T>>();

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

            InitializeWriteMethodsChache();
            InitializeReadMethodsCache();
        }

        private void InitializeReadMethodsCache()
        {
            _readMethodsCache.Add(typeof(sbyte), reader => (T)Enum.ToObject(typeof(T), _sbyteConverter.Read(reader)));
            _readMethodsCache.Add(typeof(byte), reader => (T)Enum.ToObject(typeof(T), _byteConverter.Read(reader)));
            _readMethodsCache.Add(typeof(short), reader => (T)Enum.ToObject(typeof(T), _shortConverter.Read(reader)));
            _readMethodsCache.Add(typeof(ushort), reader => (T)Enum.ToObject(typeof(T), _ushortConverter.Read(reader)));
            _readMethodsCache.Add(typeof(int), reader => (T)Enum.ToObject(typeof(T), _intConverter.Read(reader)));
            _readMethodsCache.Add(typeof(uint), reader => (T)Enum.ToObject(typeof(T), _uintConverter.Read(reader)));
            _readMethodsCache.Add(typeof(long), reader => (T)Enum.ToObject(typeof(T), _longConverter.Read(reader)));
            _readMethodsCache.Add(typeof(ulong), reader => (T)Enum.ToObject(typeof(T), _ulongConverter.Read(reader)));
        }

        private void InitializeWriteMethodsChache()
        {
            _writeMethodsCache.Add(typeof(sbyte), (value, writer) => _sbyteConverter.Write(value.ToSByte(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(byte), (value, writer) => _byteConverter.Write(value.ToByte(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(short), (value, writer) => _shortConverter.Write(value.ToInt16(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(ushort), (value, writer) => _ushortConverter.Write(value.ToUInt16(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(int), (value, writer) => _intConverter.Write(value.ToInt32(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(uint), (value, writer) => _uintConverter.Write(value.ToUInt32(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(long), (value, writer) => _longConverter.Write(value.ToInt64(CultureInfo.InvariantCulture), writer));
            _writeMethodsCache.Add(typeof(ulong), (value, writer) => _ulongConverter.Write(value.ToUInt64(CultureInfo.InvariantCulture), writer));
        }

        static EnumConverter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw ExceptionHelper.EnumExpected(enumTypeInfo);
            }
        }

        public void Write(T value, IMsgPackWriter writer)
        {
            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));

            Action<T, IMsgPackWriter> writeMethod;
            if (_writeMethodsCache.TryGetValue(enumUnderlyingType, out writeMethod))
            {
                writeMethod(value, writer);
            }
            else
            {
                throw ExceptionHelper.UnexpectedEnumUnderlyingType(enumUnderlyingType);
            }
        }
        
        public T Read(IMsgPackReader reader)
        {
            var enumUnderlyingType = Enum.GetUnderlyingType(typeof(T));
            Func<IMsgPackReader, T> readMethod;
            if (_readMethodsCache.TryGetValue(enumUnderlyingType, out readMethod))
            {
                return readMethod(reader);
            }

            throw ExceptionHelper.UnexpectedEnumUnderlyingType(enumUnderlyingType);
        }
    }
}