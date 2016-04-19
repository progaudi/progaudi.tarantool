using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace TarantoolDnx.MsgPack
{
    public class MsgPackReader : IMsgPackReader
    {
        private readonly IBytesReader _bytesReader;

        private readonly MsgPackContext _msgPackContext;

        public MsgPackReader(byte[] buffer, MsgPackContext context)
        {
            var memoryStream = new MemoryStream(buffer);
            _bytesReader = new BytesStreamReader(memoryStream);
            _msgPackContext = context;
        }

        public void Dispose()
        {
            _bytesReader.Dispose();
        }

        public T Read<T>()
        {
            var converter = GetConverter<T>(_msgPackContext);
            return converter.Read(_bytesReader, _msgPackContext, null);
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