using System.IO;

using TarantoolDnx.MsgPack.Converters;

namespace TarantoolDnx.MsgPack
{
    internal class MsgPackStreamWriter : IMsgPackWriter
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
            Write((byte) dataType);
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

        public void WriteArrayHeader(uint length)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte) ((byte) DataTypes.FixArray + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Array16);
                IntConverter.WriteValue((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Array32);
                IntConverter.WriteValue((uint) length, this);
            }
        }

        public void WriteMapHeaderAndLength(uint length)
        {
            if (length <= 15)
            {
                IntConverter.WriteValue((byte) ((byte) DataTypes.FixMap + length), this);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                Write(DataTypes.Map16);
                IntConverter.WriteValue((ushort) length, this);
            }
            else
            {
                Write(DataTypes.Map32);
                IntConverter.WriteValue((uint) length, this);
            }
        }
    }
}