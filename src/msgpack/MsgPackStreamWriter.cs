using System.IO;

namespace TarantoolDnx.MsgPack
{
    public class MsgPackStreamWriter : IMsgPackWriter
    {
        private readonly Stream _stream;

        private readonly bool _disposeStream;

        public MsgPackStreamWriter(Stream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public void Write(DataTypes dataType)
        {
            Write((byte)dataType);
        }

        public void Write(byte value)
        {
            _stream.WriteByte(value);
        }

        public void Write(byte[] array)
        {
            _stream.WriteAsync(array, 0, array.Length);
        }

        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }
    }
}