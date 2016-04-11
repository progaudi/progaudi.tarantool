using System;
using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class ReadOnlyMapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
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
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TMap));
        }
    }
}