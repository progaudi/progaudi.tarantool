using System;
using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class ReadOnlyListConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IReadOnlyList<TElement>
    {
        public override void Write(TArray value, Stream stream, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, stream, context);
                return;
            }

            WriteArrayHeaderAndLength(value.Count, stream);
            var elementConverter = context.GetConverter<TElement>();
            ValidateConverter(elementConverter);

            foreach (var element in value)
            {
                elementConverter.Write(element, stream, context);
            }
        }

        public override TArray Read(Stream stream, MsgPackContext context, Func<TArray> creator)
        {
            throw ExceptionUtils.CantReadReadOnlyCollection(typeof(TArray));
        }
    }
}