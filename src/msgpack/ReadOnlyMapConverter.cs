using System;
using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class ReadOnlyMapConverter<TMap, TKey, TValue> : MapConverterBase<TMap, TKey, TValue>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public override void Write(TMap value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            WriteMapHeaderAndLength(value.Count, stream);
            var keyConverter = settings.GetConverter<TKey>();
            var valueConverter = settings.GetConverter<TValue>();

            ValidateConverters(keyConverter, valueConverter);

            foreach (var element in value)
            {
                keyConverter.Write(element.Key, stream, settings);
                valueConverter.Write(element.Value, stream, settings);
            }
        }

        public override TMap Read(Stream stream, MsgPackSettings settings, Func<TMap> creator)
        {
            throw new System.NotImplementedException();
        }
    }
}