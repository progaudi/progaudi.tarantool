using System;
using System.Collections.Generic;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class ArrayConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IList<TElement>
    {
        private static readonly bool IsSingleDimensionArray;

        static ArrayConverter()
        {
            var type = typeof(TArray);
            IsSingleDimensionArray = type.IsArray && type.GetArrayRank() == 1 && type.GetElementType() == typeof(TElement);
        }

        public override void Write(TArray value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            writer.WriteArrayHeaderAndLength(value.Count);
            var elementConverter = context.GetConverter<TElement>();
            ValidateConverter(elementConverter);

            foreach (var element in value)
            {
                elementConverter.Write(element, writer, context);
            }
        }

        public override TArray Read(IBytesReader reader, MsgPackContext context, Func<TArray> creator)
        {
            var type = reader.ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    return default(TArray);

                case DataTypes.Array16:
                    return ReadArray(reader, context, creator, IntConverter.ReadUInt16(reader));

                case DataTypes.Array32:
                    return ReadArray(reader, context, creator, IntConverter.ReadUInt32(reader));
            }

            uint length;
            if (TryGetLengthFromFixArray(type, out length))
            {
                return ReadArray(reader, context, creator, length);
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.Array16, DataTypes.Array32, DataTypes.FixArray);
        }

        private bool TryGetLengthFromFixArray(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4);
        }

        private TArray ReadArray(IBytesReader reader, MsgPackContext context, Func<TArray> creator, uint length)
        {
            var converter = context.GetConverter<TElement>();

            ValidateConverter(converter);

            if (IsSingleDimensionArray && creator == null)
                return ReadArray(reader, context, length, converter);

            return ReadList(reader, context, creator, length, converter);
        }

        private TArray ReadArray(IBytesReader reader, MsgPackContext context, uint length, IMsgPackConverter<TElement> converter)
        {
            // ReSharper disable once RedundantCast
            var result = (TArray)(object)new TElement[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = converter.Read(reader, context, null);
            }

            return result;
        }

        private static TArray ReadList(
            IBytesReader reader,
            MsgPackContext context,
            Func<TArray> creator,
            uint length,
            IMsgPackConverter<TElement> converter)
        {
            var array = creator == null ? (TArray)context.GetObjectActivator(typeof (TArray))() : creator();

            for (var i = 0u; i < length; i++)
            {
                array.Add(converter.Read(reader, context, null));
            }

            return array;
        }
    }
}