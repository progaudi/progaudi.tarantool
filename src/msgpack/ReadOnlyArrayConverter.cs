using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class ReadOnlyArrayConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IReadOnlyList<TElement>
    {
        public override void Write(TArray value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            WriteArrayHeaderAndLength(value.Count, stream);
            var elementConverter = settings.GetConverter<TElement>();
            ValidateConverter(elementConverter);

            foreach (var element in value)
            {
                elementConverter.Write(element, stream, settings);
            }
        }
    }
}