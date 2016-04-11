using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal abstract class MapConverterBase<TMap, TKey, TValue> : IMsgPackConverter<TMap>
    {
        public abstract void Write(TMap value, Stream stream, MsgPackContext context);

        public abstract TMap Read(Stream stream, MsgPackContext context, Func<TMap> creator);

        protected void WriteMapHeaderAndLength(int length, Stream stream)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte)((byte)DataTypes.FixMap + length), stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte)DataTypes.Map16);
                IntConverter.WriteValue((ushort)length, stream);
            }
            else
            {
                stream.WriteByte((byte)DataTypes.Map32);
                IntConverter.WriteValue((uint)length, stream);
            }
        }

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