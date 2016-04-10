using System.Diagnostics;
using System.IO;

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
            settings.GetConverter<T>().Write(data, stream, settings);
            return stream.ToArray();
        }
    }
}