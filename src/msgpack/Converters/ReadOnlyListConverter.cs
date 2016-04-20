using System;
using System.Collections.Generic;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class ReadOnlyListConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IReadOnlyList<TElement>
    {
        public override void Write(TArray value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            writer.WriteArrayHeaderAndLength((uint) value.Count);
            var elementConverter = context.GetConverter<TElement>();
            ValidateConverter(elementConverter);

            foreach (var element in value)
            {
                elementConverter.Write(element, writer, context);
            }
        }

        public override TArray Read(IBytesReader reader, MsgPackContext context, Func<TArray> creator)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TArray));
        }
    }
}