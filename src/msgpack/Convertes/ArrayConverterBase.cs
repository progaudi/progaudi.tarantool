using System;

namespace TarantoolDnx.MsgPack.Convertes
{
    internal abstract class ArrayConverterBase<TArray, TElement> : IMsgPackConverter<TArray>
    {
        public abstract void Write(TArray value, IMsgPackWriter writer, MsgPackContext context);

        public abstract TArray Read(IMsgPackReader reader, MsgPackContext context, Func<TArray> creator);

        protected void WriteArrayHeaderAndLength(int length, IMsgPackWriter writer)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte)((byte)DataTypes.FixArray + length), writer);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                writer.Write(DataTypes.Array16);
                IntConverter.WriteValue((ushort)length, writer);
            }
            else
            {
                writer.Write(DataTypes.Array32);
                IntConverter.WriteValue((uint)length, writer);
            }
        }

        protected static void ValidateConverter(IMsgPackConverter<TElement> elementConverter)
        {
            if (elementConverter == null)
            {
                throw ExceptionUtils.NoConverterForCollectionElement(typeof(TElement), "element");
            }
        }
    }
}