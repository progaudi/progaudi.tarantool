using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class BinaryConverter : IMsgPackConverter<byte[]>
    {
        public void Write(byte[] value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            WriteBinaryHeaderAndLength(value.Length, stream);

            stream.Write(value, 0, value.Length);
        }

        public byte[] Read(Stream stream, MsgPackSettings settings, Func<byte[]> creator)
        {
            var type = (DataTypes)stream.ReadByte();

            byte[] buffer;
            switch (type)
            {
                case DataTypes.Bin8:
                    buffer = new byte[IntConverter.ReadUInt8(stream)];
                    break;
                case DataTypes.Bin16:
                    buffer = new byte[IntConverter.ReadUInt16(stream)];
                    break;
                case DataTypes.Bin32:
                    buffer = new byte[IntConverter.ReadUInt32(stream)];
                    break;
                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.Bin8, DataTypes.Bin16, DataTypes.Bin32);
            }

            var read = stream.Read(buffer, 0, buffer.Length);
            if (read < buffer.Length)
                throw ExceptionUtils.NotEnoughBytes(read, buffer.Length);

            return buffer;
        }

        private void WriteBinaryHeaderAndLength(int length, Stream stream)
        {
            if (length <= byte.MaxValue)
            {
                stream.WriteByte((byte)DataTypes.Bin8);
                IntConverter.WriteValue((byte) length, stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte)DataTypes.Bin16);
                IntConverter.WriteValue((ushort)length, stream);
            }
            else
            {
                stream.WriteByte((byte)DataTypes.Bin32);
                IntConverter.WriteValue((uint)length, stream);
            }
        }
    }
}