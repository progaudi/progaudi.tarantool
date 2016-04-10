using System.Collections.Generic;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class ArrayConverter<TArray, TElement> : IMsgPackConverter<TArray>
        where TArray : IReadOnlyList<TElement>
    {
        public void Write(TArray value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            WriteArrayHeaderAndLength(value.Count, stream);
            var elementConverter = settings.GetConverter<TElement>();
            foreach (var element in value)
            {
                elementConverter.Write(element, stream, settings);
            }
        }

        private void WriteArrayHeaderAndLength(int length, Stream stream)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte)((byte)DataTypes.FixArray + length), stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte)DataTypes.Array16);
                IntConverter.WriteValue((ushort)length, stream);
            }
            else
            {
                stream.WriteByte((byte)DataTypes.Array32);
                IntConverter.WriteValue((uint)length, stream);
            }
        }
    }
}