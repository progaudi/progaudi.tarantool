using System;
using System.Collections.Generic;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class MapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IDictionary<TKey, TValue>
    {
        public override void Write(TMap value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            writer.WriteMapHeaderAndLength(value.Count);
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();

            ValidateConverters(keyConverter, valueConverter);

            foreach (var element in value)
            {
                keyConverter.Write(element.Key, writer, context);
                valueConverter.Write(element.Value, writer, context);
            }
        }

        public override TMap Read(IBytesReader reader, MsgPackContext context, Func<TMap> creator)
        {
            var type = reader.ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    return default(TMap);

                case DataTypes.Map16:
                    return ReadMap(reader, context, creator, IntConverter.ReadUInt16(reader));

                case DataTypes.Map32:
                    return ReadMap(reader, context, creator, IntConverter.ReadUInt32(reader));
            }

            uint length;
            if (TryGetLengthFromFixMap(type, out length))
                return ReadMap(reader, context, creator, length);

            throw ExceptionUtils.BadTypeException(type, DataTypes.Map16, DataTypes.Map32, DataTypes.FixMap);
        }

        private bool TryGetLengthFromFixMap(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4);
        }

        private TMap ReadMap(IBytesReader reader, MsgPackContext context, Func<TMap> creator, uint length)
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