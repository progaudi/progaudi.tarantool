using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace TarantoolDnx.MsgPack.Convertes
{
    [DebuggerStepThrough]
    public static class MsgPackConverter
    {
        public static byte[] Serialize<T>(T data)
        {
            return Serialize(data, new MsgPackContext());
        }

        public static byte[] Serialize<T>(T data, [NotNull]MsgPackContext context)
        {
            var memoryStream = new MemoryStream();
            using (var wrapper = new StreamWrapper(memoryStream))
            {
                var converter = GetConverter<T>(context);

                converter.Write(data, wrapper, context);
                return memoryStream.ToArray();
            }
        }

        public static T Deserialize<T>(byte[] data)
        {
            return Deserialize<T>(data, new MsgPackContext());
        }

        public static T Deserialize<T>(byte[] data, [NotNull]MsgPackContext context)
        {
            return Deserialize<T>(data, context, null);
        }

        private static T Deserialize<T>(byte[] data, [NotNull]MsgPackContext context, Func<T> creator)
        {
            var memoryStream = new MemoryStream(data);
            using (var wrapper = new StreamWrapper(memoryStream))
            {
                var converter = GetConverter<T>(context);

                return converter.Read(wrapper, context, creator);
            }
        }

        [NotNull]
        private static IMsgPackConverter<T> GetConverter<T>(MsgPackContext context)
        {
            var converter = context.GetConverter<T>();

            if (converter == null)
            {
                throw new SerializationException($"Provide converter for {typeof(T).Name}");
            }
            return converter;
        }
    }
}