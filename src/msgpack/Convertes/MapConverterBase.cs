using System;

namespace TarantoolDnx.MsgPack.Convertes
{
    internal abstract class MapConverterBase<TMap, TKey, TValue> : IMsgPackConverter<TMap>
    {
        public abstract void Write(TMap value, IMsgPackWriter writer, MsgPackContext context);

        public abstract TMap Read(IMsgPackReader reader, MsgPackContext context, Func<TMap> creator);

        protected void WriteMapHeaderAndLength(int length, IMsgPackWriter reader)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte)((byte)DataTypes.FixMap + length), reader);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                reader.Write(DataTypes.Map16);
                IntConverter.WriteValue((ushort)length, reader);
            }
            else
            {
                reader.Write(DataTypes.Map32);
                IntConverter.WriteValue((uint)length, reader);
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