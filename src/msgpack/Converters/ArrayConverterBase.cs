using System;

namespace TarantoolDnx.MsgPack.Converters
{
    internal abstract class ArrayConverterBase<TArray, TElement> : IMsgPackConverter<TArray>
    {
        public abstract void Write(TArray value, IBytesWriter writer, MsgPackContext context);

        public abstract TArray Read(IBytesReader reader, MsgPackContext context, Func<TArray> creator);

        protected static void ValidateConverter(IMsgPackConverter<TElement> elementConverter)
        {
            if (elementConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TElement), "element");
            }
        }
    }
}