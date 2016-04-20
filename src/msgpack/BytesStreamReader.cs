using System.IO;
using TarantoolDnx.MsgPack.Converters;

namespace TarantoolDnx.MsgPack
{
    internal class BytesStreamReader : IBytesReader
    {
        private readonly Stream _stream;

        private readonly bool _disposeStream;

        public BytesStreamReader(Stream stream, bool disposeStream = true)
        {
            _stream = stream;
            _disposeStream = disposeStream;
        }

        public DataTypes ReadDataType()
        {
            return (DataTypes) ReadByte();
        }

        public byte ReadByte()
        {
            var temp = _stream.ReadByte();
            if (temp == -1)
                throw ExceptionUtils.NotEnoughBytes(0, 1);

            return (byte) temp;
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

        public bool ReadArrayLengthOrNull(out uint length)
        {
            var type = ReadDataType();
            switch (type)
            {
                case DataTypes.Null:
                    length = 0;
                    return false;
                case DataTypes.Array16:
                    length = IntConverter.ReadUInt16(this);
                    return true;

                case DataTypes.Array32:
                    length = IntConverter.ReadUInt32(this);
                    return true;
            }

            if (TryGetLengthFromFixArray(type, out length))
            {
                return true;
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.Array16, DataTypes.Array32, DataTypes.FixArray);
        }

        public bool ReadMapLengthOrNull(out uint length)
        {
            var type = ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    length = 0;
                    return false;
                case DataTypes.Map16:
                    length = IntConverter.ReadUInt16(this);
                    return true;

                case DataTypes.Map32:
                    length = IntConverter.ReadUInt32(this);
                    return true;
            }

            if (TryGetLengthFromFixMap(type, out length))
                return true;

            throw ExceptionUtils.BadTypeException(type, DataTypes.Map16, DataTypes.Map32, DataTypes.FixMap);
        }


        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }

        private static bool TryGetLengthFromFixArray(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4);
        }

        private bool TryGetLengthFromFixMap(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4);
        }
    }
}