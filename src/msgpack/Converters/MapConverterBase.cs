using System;

namespace TarantoolDnx.MsgPack.Converters
{
    internal abstract class MapConverterBase<TMap, TKey, TValue> : IMsgPackConverter<TMap>
    {
        public abstract void Write(TMap value, IMsgPackWriter writer, MsgPackContext context);

        public abstract TMap Read(IMsgPackReader reader, MsgPackContext context, Func<TMap> creator);

        protected void ValidateConverters(IMsgPackConverter<TKey> keyConverter, IMsgPackConverter<TValue> valueConverter)
        {
            if (keyConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TKey), "key");
            }

            if (valueConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TValue), "value");
            }
        }
    }
}