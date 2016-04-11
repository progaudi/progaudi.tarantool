using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    public class StreamWrapper : IMsgPackReader, IMsgPackWriter, IDisposable
    {
        private readonly Stream _stream;

        private readonly bool _disposeStream;

        public StreamWrapper(Stream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public DataTypes ReadDataType()
        {
            return (DataTypes)ReadByte();
        }

        public byte ReadByte()
        {
            var temp = _stream.ReadByte();
            if (temp == -1)
                throw ExceptionUtils.NotEnoughBytes(0, 1);

            return (byte)temp;
        }

        public void ReadBytes(byte[] buffer)
        {
            var read = _stream.Read(buffer, 0, buffer.Length);
            if (read < buffer.Length)
                throw ExceptionUtils.NotEnoughBytes(read, buffer.Length);
        }

        public void Seek(int offset, SeekOrigin origin)
        {
            _stream.Seek(offset, origin);
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