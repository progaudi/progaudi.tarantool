using System;
using System.IO;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
{
    internal abstract class ArrayConverterBase<TArray, TElement> : IMsgPackConverter<TArray>
    {
        public abstract void Write(TArray value, Stream stream, MsgPackSettings settings);

        public abstract TArray Read(Stream stream, MsgPackSettings settings, Func<TArray> creator);

        protected void WriteArrayHeaderAndLength(int length, Stream stream)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte) ((byte) DataTypes.FixArray + length), stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte) DataTypes.Array16);
                IntConverter.WriteValue((ushort) length, stream);
            }
            else
            {
                stream.WriteByte((byte) DataTypes.Array32);
                IntConverter.WriteValue((uint) length, stream);
            }
        }

        protected static void ValidateConverter(IMsgPackConverter<TElement> elementConverter)
        {
            if (elementConverter == null)
            {
                throw new SerializationException($"Provide converter for element: {typeof(TElement).Name}");
            }
        }
    }
}