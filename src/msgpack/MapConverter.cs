using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
{
    internal class ReadOnlyMapConverter<TMap, TKey, TValue> : IMsgPackConverter<TMap>
        where TMap : IReadOnlyDictionary<TKey, TValue>
    {
        public void Write(TMap value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            WriteMapHeaderAndLength(value.Count, stream);
            var keyConverter = settings.GetConverter<TKey>();
            var valueConverter = settings.GetConverter<TValue>();

            if (keyConverter == null)
            {
                throw new SerializationException($"Provide serializer for keys");
            }

            if (valueConverter == null)
            {
                throw new SerializationException($"Provide serializer for values");
            }

            foreach (var element in value)
            {
                keyConverter.Write(element.Key, stream, settings);
                valueConverter.Write(element.Value, stream, settings);
            }
        }

        private void WriteMapHeaderAndLength(int length, Stream stream)
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
    }
}