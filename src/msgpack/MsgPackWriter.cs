using System.IO;
using System.Runtime.Serialization;

using JetBrains.Annotations;

namespace TarantoolDnx.MsgPack
{
    public class MsgPackWriter : IMsgPackWriter
    {
        private readonly MemoryStream _innerStream;
        private readonly IBytesWriter _bytesWriter;

        private readonly MsgPackContext _msgPackContext;

        public MsgPackWriter( MsgPackContext msgPackContext)
        {
            _innerStream = new MemoryStream();
            _bytesWriter = new BytesStreamWriter(_innerStream);
            _msgPackContext = msgPackContext;
        }

        public void Dispose()
        {
            _bytesWriter.Dispose();
        }

        public void Write<T>(T value)
        {
            var converter = GetConverter<T>(_msgPackContext);
            converter.Write(value, _bytesWriter, _msgPackContext);
        }

        public byte[] ToArray()
        {
            return _innerStream.ToArray();
        }

        public IMsgPackWriter Clone()
        {
            return new MsgPackWriter(_msgPackContext);
        }

        public void WriteRaw(byte[] headeAndBodyBuffer)
        {
            _bytesWriter.Write(headeAndBodyBuffer);
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