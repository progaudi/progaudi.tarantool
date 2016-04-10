using System.IO;
using System.Text;

namespace TarantoolDnx.MsgPack
{
    internal class StringConverter : IMsgPackConverter<string>
    {
        public void Write(string value, Stream stream, MsgPackSettings settings)
        {
            if (value == null)
            {
                settings.NullConverter.Write(value, stream, settings);
                return;
            }

            var data = Encoding.UTF8.GetBytes(value);

            WriteStringHeaderAndLength(stream, data.Length);

            stream.Write(data, 0, data.Length);
        }

        private void WriteStringHeaderAndLength(Stream stream, int length)
        {
            if (length <= 31)
            {
                stream.WriteByte((byte)(((byte)DataTypes.FixStr + length) % 256));
                return;
            }

            if (length <= byte.MaxValue)
            {
                stream.WriteByte((byte)DataTypes.Str8);
                IntConverter.WriteValue((byte)length, stream);
                return;
            }

            if (length <= IntLimits.Max16Bit)
            {
                stream.WriteByte((byte)DataTypes.Str16);
                IntConverter.WriteValue((ushort)length, stream);
            }
            else
            {
                stream.WriteByte((byte)DataTypes.Str32);
                IntConverter.WriteValue((uint)length, stream);
            }
        }
    }
}