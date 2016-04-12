using System;

namespace TarantoolDnx.MsgPack.Converters
{
    internal class BoolConverter : IMsgPackConverter<bool>
    {
        public void Write(bool value, IMsgPackWriter writer, MsgPackContext context)
        {
            writer.Write(value ? DataTypes.True : DataTypes.False);
        }

        public bool Read(IMsgPackReader reader, MsgPackContext context, Func<bool> creator)
        {
            var type = reader.ReadDataType();

            switch (type)
            {
                case DataTypes.True:
                    return true;

                case DataTypes.False:
                    return false;

                default:
                    throw ExceptionUtils.BadTypeException(type, DataTypes.True, DataTypes.False);
            }
        }
    }
}