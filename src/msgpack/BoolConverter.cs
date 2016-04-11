using System;
using System.IO;

namespace TarantoolDnx.MsgPack
{
    internal class BoolConverter : IMsgPackConverter<bool>
    {
        public void Write(bool value, Stream stream, MsgPackContext context)
        {
            stream.WriteByte((byte) (value ? DataTypes.True : DataTypes.False));
        }

        public bool Read(Stream stream, MsgPackContext context, Func<bool> creator)
        {
            var header = (DataTypes) stream.ReadByte();
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