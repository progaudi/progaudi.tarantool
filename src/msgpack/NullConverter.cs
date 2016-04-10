using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class NullConverter : IMsgPackConverter<object>
    {
        public void Write(object value, Stream stream, MsgPackSettings settings)
        {
            stream.WriteByte((byte)DataTypes.Null);
        }
    }
}