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