using System;
using System.IO;
using System.Text;

namespace TarantoolDnx.MsgPack
{
    internal class StringConverter : IMsgPackConverter<string>
    {
        public void Write(string value, Stream stream, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, stream, context);
                return;
            }

            var data = Encoding.UTF8.GetBytes(value);

            WriteStringHeaderAndLength(stream, data.Length);

            stream.Write(data, 0, data.Length);
        }

        public string Read(Stream stream, MsgPackContext context, Func<string> creator)
        {
            var type = (DataTypes) stream.ReadByte();

            uint length;
            if (TryGetFixstrLength(type, out length))
            {
                return ReadString(stream, length);
            }

            switch (type)
            {
                case DataTypes.Null:
                    return null;
                case DataTypes.Str8:
                    return ReadString(stream, IntConverter.ReadUInt8(stream));
                case DataTypes.Str16:
                    return ReadString(stream, IntConverter.ReadUInt16(stream));
                case DataTypes.Str32:
                    return ReadString(stream, IntConverter.ReadUInt32(stream));
                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.FixStr, DataTypes.Str8, DataTypes.Str16, DataTypes.Str32);
            }
        }

        private string ReadString(Stream stream, uint length)
        {
            var buffer = BinaryConverter.ReadByteArray(stream, length);

            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        private bool TryGetFixstrLength(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixStr;
            return (type & DataTypes.FixStr) == DataTypes.FixStr;
        }

        private void WriteStringHeaderAndLength(Stream stream, int length)
        {
            if (length <= 31)
            {
                stream.WriteByte((byte) (((byte) DataTypes.FixStr + length) % 256));
                return;
            }

            if (length <= byte.MaxValue)
            {
                stream.WriteByte((byte) DataTypes.Str8);
                IntConverter.WriteValue((byte) length, stream);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                stream.WriteByte((byte) DataTypes.Str16);
                IntConverter.WriteValue((ushort) length, stream);
            }
            else
            {
                stream.WriteByte((byte) DataTypes.Str32);
                IntConverter.WriteValue((uint) length, stream);
            }
        }
    }
}