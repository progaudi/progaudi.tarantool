using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class NullConverter : IMsgPackConverter<object>
    {
        public void Write(object value, Stream stream, MsgPackSettings settings)
        {
            stream.WriteByte((byte) DataTypes.Null);
        }

        public object Read(Stream stream, MsgPackSettings settings, Func<object> creator)
        {
            var type = (DataTypes) stream.ReadByte();
            if (type == DataTypes.Null)
                return null;

            throw ExceptionUtils.BadTypeException(type, DataTypes.Null);
        }
    }
}