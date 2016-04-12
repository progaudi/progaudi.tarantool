using System;
using System.Collections.Generic;

namespace TarantoolDnx.MsgPack.Convertes
{
    internal class MapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IDictionary<TKey, TValue>
    {
        public override void Write(TMap value, IMsgPackWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            WriteMapHeaderAndLength(value.Count, writer);
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();

            ValidateConverters(keyConverter, valueConverter);

            foreach (var element in value)
            {
                keyConverter.Write(element.Key, writer, context);
                valueConverter.Write(element.Value, writer, context);
            }
        }

        public override TMap Read(IMsgPackReader reader, MsgPackContext context, Func<TMap> creator)
        {
            var type = reader.ReadDataType();

            uint length;
            if (TryGetLengthFromFixMap(type, out length))
                return ReadMap(reader, context, creator, length);

            switch (type)
            {
                case DataTypes.Null:
                    return default(TMap);

                case DataTypes.Map16:
                    return ReadMap(reader, context, creator, IntConverter.ReadUInt16(reader));

                case DataTypes.Map32:
                    return ReadMap(reader, context, creator, IntConverter.ReadUInt32(reader));

                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.Map16, DataTypes.Map32, DataTypes.FixMap);
            }
        }

        private bool TryGetLengthFromFixMap(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixMap;
            return (type & DataTypes.FixMap) == DataTypes.FixMap;
        }

        private TMap ReadMap(IMsgPackReader reader, MsgPackContext context, Func<TMap> creator, uint length)
        {
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();

            ValidateConverters(keyConverter, valueConverter);

            var map = creator == null ? (TMap)context.GetObjectActivator(typeof (TMap))() : creator();

            for (var i = 0u; i < length; i++)
            {
                var key = keyConverter.Read(reader, context, null);
                var value = valueConverter.Read(reader, context, null);

                map[key] = value;
            }

            return map;
        }
    }
}