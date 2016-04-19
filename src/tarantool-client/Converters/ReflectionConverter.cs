using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;
// ReSharper disable once RedundantUsingDirective
using System.Reflection;
using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class ReflectionConverter : IMsgPackConverter<object>
    {
        public void Write(object value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            var converter = GetConverter(context, value.GetType());

            var methodDefinition = typeof(IMsgPackConverter<>).MakeGenericType(value.GetType()).GetMethod(
                "Write",
                new[] { value.GetType(), typeof(IBytesWriter), typeof(MsgPackContext) });

            methodDefinition.Invoke(converter, new[] { value, writer, context });
        }

        public object Read(IBytesReader reader, MsgPackContext context, Func<object> creator)
        {
            var msgPackType = reader.ReadDataType();

            Type type;
            switch (msgPackType)
            {
                case DataTypes.Null:
                    return null;

                case DataTypes.False:
                    return false;

                case DataTypes.True:
                    return true;

                case DataTypes.Single:
                    type = typeof(float);
                    break;

                case DataTypes.Double:
                    type = typeof(double);
                    break;

                case DataTypes.UInt8:
                    type = typeof(byte);
                    break;

                case DataTypes.UInt16:
                    type = typeof(ushort);
                    break;

                case DataTypes.UInt32:
                    type = typeof(uint);
                    break;

                case DataTypes.UInt64:
                    type = typeof(ulong);
                    break;

                case DataTypes.Int8:
                    type = typeof(sbyte);
                    break;

                case DataTypes.Int16:
                    type = typeof(short);
                    break;

                case DataTypes.Int32:
                    type = typeof(int);
                    break;

                case DataTypes.Int64:
                    type = typeof(long);
                    break;

                case DataTypes.Array16:
                    type = typeof(object[]);
                    break;

                case DataTypes.Array32:
                    type = typeof(object[]);
                    break;

                case DataTypes.Map16:
                    type = typeof(Dictionary<object, object>);
                    break;

                case DataTypes.Map32:
                    type = typeof(Dictionary<object, object>);
                    break;

                case DataTypes.Str8:
                    type = typeof(string);
                    break;

                case DataTypes.Str16:
                    type = typeof(string);
                    break;

                case DataTypes.Str32:
                    type = typeof(string);
                    break;

                case DataTypes.Bin8:
                    type = typeof(byte[]);
                    break;

                case DataTypes.Bin16:
                    type = typeof(byte[]);
                    break;

                case DataTypes.Bin32:
                    type = typeof(byte[]);
                    break;

                default:
                    type = TryInferFromFixedLength(msgPackType);
                    break;
            }

            reader.Seek(-1, SeekOrigin.Current);
            var converter = GetConverter(context, type);
            var methodDefinition = typeof(IMsgPackConverter<>).MakeGenericType(type).GetMethod(
                "Read",
                new[] { typeof(IBytesReader), typeof(MsgPackContext), typeof(Func<>).MakeGenericType(type) });

            return methodDefinition.Invoke(converter, new object[] { reader, context, null });
        }

        private Type TryInferFromFixedLength(DataTypes msgPackType)
        {
            if ((msgPackType & DataTypes.PositiveFixNum) == msgPackType)
                return typeof(byte);

            if ((msgPackType & DataTypes.NegativeFixNum) == DataTypes.NegativeFixNum)
                return typeof(sbyte);

            if ((msgPackType & DataTypes.FixArray) == DataTypes.FixArray)
                return typeof(object[]);

            if ((msgPackType & DataTypes.FixStr) == DataTypes.FixStr)
                return typeof(string);

            if ((msgPackType & DataTypes.FixMap) == DataTypes.FixMap)
                return typeof(Dictionary<object, object>);

            throw new SerializationException($"Can't infer type for msgpack type: {msgPackType:G} (0x{msgPackType:X})");
        }

        [NotNull]
        private static object GetConverter(MsgPackContext context, Type type)
        {
            var methodDefinition = typeof(MsgPackContext).GetMethod(nameof(MsgPackContext.GetConverter));
            var concreteMethod = methodDefinition.MakeGenericMethod(type);
            var converter = concreteMethod.Invoke(context, null);
            if (converter == null)
                throw new SerializationException($"Please, provide convertor for {type.Name}");
            return converter;
        }
    }
}