using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class BinaryConverter : IMsgPackConverter<byte[]>
    {
        public void Write(byte[] value, Stream stream, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, stream, context);
                return;
            }

            WriteBinaryHeaderAndLength(value.Length, stream);

            stream.Write(value, 0, value.Length);
        }

        // We will have problem with binary blobs greater than int.MaxValue bytes.
        public byte[] Read(Stream stream, MsgPackContext context, Func<byte[]> creator)
        {
            var type = (DataTypes) stream.ReadByte();

            uint length;
            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.Bin8:
                    length = IntConverter.ReadUInt8(stream);
                    break;
                case DataTypes.Bin16:
                    length = IntConverter.ReadUInt16(stream);
                    break;
                case DataTypes.Bin32:
                    length = IntConverter.ReadUInt32(stream);
                    break;
                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.Bin8, DataTypes.Bin16, DataTypes.Bin32, DataTypes.Null);
            }

            return ReadByteArray(stream, length);
        }

        internal static byte[] ReadByteArray(Stream stream, uint length)
        {
            var buffer = new byte[length];
            var read = stream.Read(buffer, 0, buffer.Length);
            if (read < buffer.Length)
                throw ExceptionUtils.NotEnoughBytes(read, buffer.Length);

            return buffer;
        }

        private void WriteBinaryHeaderAndLength(int length, Stream stream)
        {
            if (length <= byte.MaxValue)
            {
                stream.WriteByte((byte) DataTypes.Bin8);
                IntConverter.WriteValue((byte) length, stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte) DataTypes.Bin16);
                IntConverter.WriteValue((ushort) length, stream);
            }
            else
            {
                stream.WriteByte((byte) DataTypes.Bin32);
                IntConverter.WriteValue((uint) length, stream);
            }
        }
    }
}