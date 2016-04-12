using System;

namespace TarantoolDnx.MsgPack.Convertes
{
    internal class BoolConverter : IMsgPackStructConverter<bool>
    {
        public void Write(bool value, IMsgPackWriter writer, MsgPackContext context)
        {
            writer.Write(value ? DataTypes.True : DataTypes.False);
        }

        public bool Read(IMsgPackReader reader, MsgPackContext context, Func<bool> creator)
        {
            return ReadWithoutTypeReading(reader.ReadDataType(), reader, context, creator);
        }

        public bool ReadWithoutTypeReading(DataTypes type, IMsgPackReader reader, MsgPackContext context, Func<bool> creator)
        {
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