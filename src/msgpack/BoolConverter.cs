using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class BoolConverter : IMsgPackConverter<bool>
    {
        public void Write(bool value, Stream stream, MsgPackSettings settings)
        {
            stream.WriteByte((byte)(value ? DataTypes.True : DataTypes.False));
        }
    }
}