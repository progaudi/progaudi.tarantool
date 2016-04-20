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

        public uint? ReadArrayLengthOrNull()
        {
            var type = ReadDataType();
            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.Array16:
                    return IntConverter.ReadUInt16(this);

                case DataTypes.Array32:
                    return IntConverter.ReadUInt32(this);
            }

            var length = TryGetLengthFromFixArray(type);

            if (length.HasValue)
            {
                return length;
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.Array16, DataTypes.Array32, DataTypes.FixArray);
        }

        public uint? ReadMapLengthOrNull()
        {
            var type = ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.Map16:
                    return IntConverter.ReadUInt16(this);

                case DataTypes.Map32:
                    return IntConverter.ReadUInt32(this);
            }

            var length = TryGetLengthFromFixMap(type);
            if (length.HasValue)
                return length.Value;

            throw ExceptionUtils.BadTypeException(type, DataTypes.Map16, DataTypes.Map32, DataTypes.FixMap);
        }


        public void Dispose()
        {
            if (_disposeStream)
                _stream.Dispose();
        }

        private static uint? TryGetLengthFromFixArray(DataTypes type)
        {
            var length = type - DataTypes.FixArray;
            return type.GetHighBits(4) == DataTypes.FixArray.GetHighBits(4) ? length : (uint?) null;
        }

        private static uint? TryGetLengthFromFixMap(DataTypes type)
        {
            var length = type - DataTypes.FixMap;
            return type.GetHighBits(4) == DataTypes.FixMap.GetHighBits(4) ? length : (uint?) null;
        }
    }
}