using System;
using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class MapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IDictionary<TKey, TValue>
    {
        public override void Write(TMap value, Stream stream, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, stream, context);
                return;
            }

            WriteMapHeaderAndLength(value.Count, stream);
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();

            ValidateConverters(keyConverter, valueConverter);

            foreach (var element in value)
            {
                keyConverter.Write(element.Key, stream, context);
                valueConverter.Write(element.Value, stream, context);
            }
        }

        public override TMap Read(Stream stream, MsgPackContext context, Func<TMap> creator)
        {
            var type = (DataTypes)stream.ReadByte();

            uint length;
            if (TryGetLengthFromFixMap(type, out length))
                return ReadMap(stream, context, creator, length);

            switch (type)
            {
                case DataTypes.Null:
                    return default(TMap);

                case DataTypes.Map16:
                    return ReadMap(stream, context, creator, IntConverter.ReadUInt16(stream));

                case DataTypes.Map32:
                    return ReadMap(stream, context, creator, IntConverter.ReadUInt32(stream));

                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.Map16, DataTypes.Map32, DataTypes.FixMap);
            }
        }

        private bool TryGetLengthFromFixMap(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixMap;
            return (type & DataTypes.FixMap) == DataTypes.FixMap;
        }

        private TMap ReadMap(Stream stream, MsgPackContext context, Func<TMap> creator, uint length)
        {
            var keyConverter = context.GetConverter<TKey>();
            var valueConverter = context.GetConverter<TValue>();

            ValidateConverters(keyConverter, valueConverter);

            var map = creator == null ? (TMap)Activator.CreateInstance(typeof(TMap)) : creator();

            for (var i = 0u; i < length; i++)
            {
                var key = keyConverter.Read(stream, context, null);
                var value = valueConverter.Read(stream, context, null);

                map[key] = value;
            }

            return map;
        }
    }
}