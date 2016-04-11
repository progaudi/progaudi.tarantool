// ReSharper disable once RedundantUsingDirective
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace TarantoolDnx.MsgPack.Tests
{
    public class TestReflectionConverter : IMsgPackConverter<object>
    {
        public void Write(object value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            var converter = GetConverter(settings, value.GetType());

            var methodDefinition = typeof(IMsgPackConverter<>).MakeGenericType(value.GetType()).GetMethod(
                "Write",
                new[] {value.GetType(), typeof(Stream), typeof(MsgPackSettings)});

            methodDefinition.Invoke(converter, new[] {value, stream, settings});
        }

        public object Read(Stream stream, MsgPackSettings settings, Func<object> creator)
        {
            var msgPackType = (DataTypes) stream.ReadByte();

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

            stream.Seek(-1, SeekOrigin.Current);
            var converter = GetConverter(settings, type);
            var methodDefinition = typeof(IMsgPackConverter<>).MakeGenericType(type).GetMethod(
                "Read",
                new[] {typeof(Stream), typeof(MsgPackSettings), typeof(Func<>).MakeGenericType(type)});

            return methodDefinition.Invoke(converter, new object[] {stream, settings, null});
        }

        private Type TryInferFromFixedLength(DataTypes msgPackType)
        {
            if ((msgPackType & DataTypes.PositiveFixNum) == msgPackType)
                return typeof(byte);

            if ((msgPackType & DataTypes.NegativeFixnum) == DataTypes.NegativeFixnum)
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
        private static object GetConverter(MsgPackSettings settings, Type type)
        {
            var methodDefinition = typeof(MsgPackSettings).GetMethod(nameof(MsgPackSettings.GetConverter));
            var concreteMethod = methodDefinition.MakeGenericMethod(type);
            var converter = concreteMethod.Invoke(settings, null);
            if (converter == null)
                throw new SerializationException($"Please, provide convertor for {type.Name}");
            return converter;
        }
    }
}