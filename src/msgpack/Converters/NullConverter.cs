using System;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class NullConverter : IMsgPackConverter<object>
    {
        public void Write(object value, IBytesWriter writer, MsgPackContext context)
        {
            writer.Write(DataTypes.Null);
        }

        public object Read(IBytesReader reader, MsgPackContext context, Func<object> creator)
        {
            var type = reader.ReadDataType();
            if (type == DataTypes.Null)
                return null;

            throw ExceptionUtils.BadTypeException(type, DataTypes.Null);
        }
    }
}