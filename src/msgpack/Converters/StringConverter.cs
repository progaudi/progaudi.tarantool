using System;
using System.Text;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class StringConverter : IMsgPackConverter<string>
    {
        public void Write(string value, IBytesWriter writer, MsgPackContext context)
        {
            if (value == null)
            {
                context.NullConverter.Write(value, writer, context);
                return;
            }

            var data = Encoding.UTF8.GetBytes(value);

            WriteStringHeaderAndLength(writer, data.Length);

            writer.Write(data);
        }

        public string Read(IBytesReader reader, MsgPackContext context, Func<string> creator)
        {
            var type = reader.ReadDataType();

            switch (type)
            {
                case DataTypes.Null:
                    return null;

                case DataTypes.Str8:
                    return ReadString(reader, IntConverter.ReadUInt8(reader));

                case DataTypes.Str16:
                    return ReadString(reader, IntConverter.ReadUInt16(reader));

                case DataTypes.Str32:
                    return ReadString(reader, IntConverter.ReadUInt32(reader));
            }

            uint length;
            if (TryGetFixstrLength(type, out length))
            {
                return ReadString(reader, length);
            }

            throw ExceptionUtils.BadTypeException(type, DataTypes.FixStr, DataTypes.Str8, DataTypes.Str16, DataTypes.Str32);
        }

        private string ReadString(IBytesReader reader, uint length)
        {
            var buffer = BinaryConverter.ReadByteArray(reader, length);

            return Encoding.UTF8.GetString(buffer, 0, buffer.Length);
        }

        private bool TryGetFixstrLength(DataTypes type, out uint length)
        {
            length = type - DataTypes.FixStr;
            return type.GetHighBits(3) == DataTypes.FixStr.GetHighBits(3);
        }

        private void WriteStringHeaderAndLength(IBytesWriter writer, int length)
        {
            if (length <= 31)
            {
                writer.Write((byte)(((byte)DataTypes.FixStr + length) % 256));
                return;
            }

            if (length <= byte.MaxValue)
            {
                writer.Write(DataTypes.Str8);
                IntConverter.WriteValue((byte)length, writer);
                return;
            }

            if (length <= ushort.MaxValue)
            {
                writer.Write(DataTypes.Str16);
                IntConverter.WriteValue((ushort)length, writer);
            }
            else
            {
                writer.Write(DataTypes.Str32);
                IntConverter.WriteValue((uint)length, writer);
            }
        }
    }
}