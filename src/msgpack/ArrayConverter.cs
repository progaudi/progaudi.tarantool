using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TarantoolDnx.MsgPack
{
    internal class ArrayConverter<TArray, TElement> : ArrayConverterBase<TArray, TElement>
        where TArray : IList<TElement>
    {
        private static readonly bool IsSingleDimensionArray;
        private static readonly ConstructorInfo ArrayConstructor;

        static ArrayConverter()
        {
            var type = typeof(TArray);
            IsSingleDimensionArray = type.IsArray && type.GetArrayRank() == 1 && type.GetElementType() == typeof(TElement);
            if (IsSingleDimensionArray)
                ArrayConstructor = type.GetConstructors()[0];
        }

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

        public override TArray Read(Stream stream, MsgPackSettings settings, Func<TArray> creator)
        {
            var type = (DataTypes)stream.ReadByte();

            uint length;
            if (TryGetLengthFromFixArray(type, out length))
                return ReadArray(stream, settings, creator, length);

            switch (type)
            {
                case DataTypes.Null:
                    return default(TArray);
                case DataTypes.Array16:
                    return ReadArray(stream, settings, creator, IntConverter.ReadUInt16(stream));
                case DataTypes.Array32:
                    return ReadArray(stream, settings, creator, IntConverter.ReadUInt32(stream));
                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.Array16, DataTypes.Array32, DataTypes.FixArray);
            }
        }

        private bool TryGetLengthFromFixArray(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixArray;
            return (type & DataTypes.FixArray) == DataTypes.FixArray;
        }

        private TArray ReadArray(Stream stream, MsgPackSettings settings, Func<TArray> creator, uint length)
        {
            var converter = settings.GetConverter<TElement>();

            ValidateConverter(converter);

            if (IsSingleDimensionArray && creator == null)
                return ReadArray(stream, settings, length, converter);

            return ReadList(stream, settings, creator, length, converter);
        }

        private TArray ReadArray(Stream stream, MsgPackSettings settings, uint length, IMsgPackConverter<TElement> converter)
        {
            var result = (TArray)ArrayConstructor.Invoke(new object[] { (int)length });

            for (var i = 0; i < length; i++)
            {
                result[i] = converter.Read(stream, settings, null);
            }
        
            return result;
        }

        private static TArray ReadList(Stream stream, MsgPackSettings settings, Func<TArray> creator, uint length,
            IMsgPackConverter<TElement> converter)
        {
            var array = creator == null ? (TArray) Activator.CreateInstance(typeof(TArray)) : creator();

            for (var i = 0u; i < length; i++)
            {
                array.Add(converter.Read(stream, settings, null));
            }

            return array;
        }
    }
}