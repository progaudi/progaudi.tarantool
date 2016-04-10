using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace TarantoolDnx.MsgPack
{
    [DebuggerStepThrough]
    public static class MsgPackConverter
    {
        public static byte[] Convert<T>(T data)
        {
            return Convert(data, new MsgPackSettings());
        }

        public static byte[] Convert<T>(T data, MsgPackSettings settings)
        {
            var stream = new MemoryStream();
            var converter = settings.GetConverter<T>();

            if (converter == null)
            {
                throw new SerializationException($"Provide converter for {typeof(T).Name}");
            }

            converter.Write(data, stream, settings);
            return stream.ToArray();
        }
    }
}