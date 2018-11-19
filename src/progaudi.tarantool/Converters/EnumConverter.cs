using System;
using System.Globalization;
using System.Reflection;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Formatters
{
    internal sealed class EnumFormatter<T> : IMsgPackFormatter<T>, IMsgPackParser<T>
        where T : struct, IConvertible
    {
        // ReSharper disable once StaticMemberInGenericType
        private static readonly int Length;
        // ReSharper disable once StaticMemberInGenericType
        private static readonly Type UnderlyingType;

        static EnumFormatter()
        {
            var enumTypeInfo = typeof(T).GetTypeInfo();
            if (!enumTypeInfo.IsEnum)
            {
                throw ExceptionHelper.EnumExpected(enumTypeInfo);
            }

            UnderlyingType = enumTypeInfo.GetEnumUnderlyingType();
            Length = Caches.Lengths.TryGetValue(UnderlyingType, out var x) ? x : throw ExceptionHelper.UnexpectedEnumUnderlyingType(UnderlyingType);
        }

        public int GetBufferSize(T value) => Length;

        public bool HasConstantSize => true;

        public int Format(Span<byte> destination, T value)
        {
            if (UnderlyingType == typeof(byte)) return MsgPackSpec.WriteFixUInt8(destination, Convert.ToByte(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(sbyte)) return MsgPackSpec.WriteFixInt8(destination, Convert.ToSByte(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(ushort)) return MsgPackSpec.WriteFixUInt16(destination, Convert.ToUInt16(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(short)) return MsgPackSpec.WriteFixInt16(destination, Convert.ToInt16(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(uint)) return MsgPackSpec.WriteFixUInt32(destination, Convert.ToUInt32(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(int)) return MsgPackSpec.WriteFixInt32(destination, Convert.ToInt32(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(ulong)) return MsgPackSpec.WriteFixUInt64(destination, Convert.ToUInt64(value, CultureInfo.InvariantCulture));
            if (UnderlyingType == typeof(long)) return MsgPackSpec.WriteFixInt64(destination, Convert.ToInt64(value, CultureInfo.InvariantCulture));
            
            // we'll fail in static ctor
            throw ExceptionHelper.UnexpectedEnumUnderlyingType(UnderlyingType);
        }

        public T Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            if (UnderlyingType == typeof(byte)) return (T) (object) MsgPackSpec.ReadFixUInt8(source, out readSize);
            if (UnderlyingType == typeof(sbyte)) return (T) (object) MsgPackSpec.ReadFixInt8(source, out readSize);
            if (UnderlyingType == typeof(ushort)) return (T) (object) MsgPackSpec.ReadFixUInt16(source, out readSize);
            if (UnderlyingType == typeof(short)) return (T) (object) MsgPackSpec.ReadFixInt16(source, out readSize);
            if (UnderlyingType == typeof(uint)) return (T) (object) MsgPackSpec.ReadFixUInt32(source, out readSize);
            if (UnderlyingType == typeof(int)) return (T) (object) MsgPackSpec.ReadFixInt32(source, out readSize);
            if (UnderlyingType == typeof(ulong)) return (T) (object) MsgPackSpec.ReadFixUInt64(source, out readSize);
            if (UnderlyingType == typeof(long)) return (T) (object) MsgPackSpec.ReadFixInt64(source, out readSize);

            // we'll fail in static ctor
            throw ExceptionHelper.UnexpectedEnumUnderlyingType(UnderlyingType);
        }
    }
}