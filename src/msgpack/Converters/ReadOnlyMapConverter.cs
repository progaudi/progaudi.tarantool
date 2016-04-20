using System;
using System.Collections.Generic;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class ReadOnlyMapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public override void Write(TMap value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            writer.WriteMapHeaderAndLength((uint) value.Count);
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
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TMap));
        }
    }
}