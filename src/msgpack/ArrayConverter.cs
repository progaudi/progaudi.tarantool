using System;
using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
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
            var type = (DataTypes)stream.ReadByte();

            uint length;
            if (TryGetLengthFromFixArray(type, out length))
                return ReadArray(stream, context, creator, length);

            switch (type)
            {
                case DataTypes.Null:
                    return default(TArray);

                case DataTypes.Array16:
                    return ReadArray(stream, context, creator, IntConverter.ReadUInt16(stream));

                case DataTypes.Array32:
                    return ReadArray(stream, context, creator, IntConverter.ReadUInt32(stream));

                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.Array16, DataTypes.Array32, DataTypes.FixArray);
            }
        }

        private bool TryGetLengthFromFixArray(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixArray;
            return (type & DataTypes.FixArray) == DataTypes.FixArray;
        }

        private TArray ReadArray(Stream stream, MsgPackContext context, Func<TArray> creator, uint length)
        {
            var converter = context.GetConverter<TElement>();

            ValidateConverter(converter);

            if (IsSingleDimensionArray && creator == null)
                return ReadArray(stream, context, length, converter);

            return ReadList(stream, context, creator, length, converter);
        }

        private TArray ReadArray(Stream stream, MsgPackContext context, uint length, IMsgPackConverter<TElement> converter)
        {
            // ReSharper disable once RedundantCast
            var result = (TArray)(object)new TElement[length];

            for (var i = 0; i < length; i++)
            {
                result[i] = converter.Read(stream, context, null);
            }

            return result;
        }

        private static TArray ReadList(
            Stream stream,
            MsgPackContext context,
            Func<TArray> creator,
            uint length,
            IMsgPackConverter<TElement> converter)
        {
            var array = creator == null ? (TArray)Activator.CreateInstance(typeof(TArray)) : creator();

            for (var i = 0u; i < length; i++)
            {
                array.Add(converter.Read(stream, context, null));
            }

            return array;
        }
    }
}