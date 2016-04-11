using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
{
    [DebuggerStepThrough]
    public static class MsgPackConverter
    {
        public static byte[] Serialize<T>(T data)
        {
            return Serialize(data, new MsgPackSettings());
        }

        public static byte[] Serialize<T>(T data, MsgPackSettings settings)
        {
            var stream = new MemoryStream();
            var converter = GetConverter<T>(settings);

            converter.Write(data, stream, settings);
            return stream.ToArray();
        }

        public static T Deserialize<T>(byte[] data)
        {
            return Deserialize<T>(data, new MsgPackSettings());
        }

        public static T Deserialize<T>(byte[] data, MsgPackSettings settings)
        {
            return Deserialize<T>(data, settings, null);
        }

        private static T Deserialize<T>(byte[] data, MsgPackSettings settings, Func<T> creator)
        {
            var stream = new MemoryStream(data);
            var converter = GetConverter<T>(settings);

            return converter.Read(stream, settings, creator);
        }

        private static IMsgPackConverter<T> GetConverter<T>(MsgPackSettings settings)
        {
            var converter = settings.GetConverter<T>();

            if (converter == null)
            {
                throw new SerializationException($"Provide converter for {typeof(T).Name}");
            }
            return converter;
        }
    }
}