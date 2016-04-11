using System;

namespace TarantoolDnx.MsgPack
{
    internal class BoolConverter : IMsgPackConverter<bool>
    {
        public void Write(bool value, IMsgPackWriter writer, MsgPackContext context)
        {
            writer.Write(value ? DataTypes.True : DataTypes.False);
        }

        public bool Read(IMsgPackReader reader, MsgPackContext context, Func<bool> creator)
        {
            var header = reader.ReadDataType();
            switch (header)
            {
                case DataTypes.True:
                    return true;

                case DataTypes.False:
                    return false;

                default:
                    throw ExceptionUtils.BadTypeException(header, DataTypes.True, DataTypes.False);
            }
        }
    }
}