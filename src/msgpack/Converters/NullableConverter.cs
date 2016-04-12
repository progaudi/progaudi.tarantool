using System;
using System.IO;

namespace TarantoolDnx.MsgPack.Converters
{
    public class NullableConverter<T> : IMsgPackConverter<T?> where T : struct
    {
        public T? Read(IMsgPackReader reader, MsgPackContext context, Func<T?> creator)
        {
            var type = reader.ReadDataType();

            if (type == DataTypes.Null)
                return null;

            var structConverter = context.GetConverter<T>();

            Func<T> nullableCreator;
            if (creator == null)
            {
                nullableCreator = null;
            }
            else
            {
                nullableCreator = () =>
                {
                    var result = creator();
                    return result ?? default(T);
                };
            }

            reader.Seek(-1, SeekOrigin.Current);

            return structConverter.Read(reader, context, nullableCreator);
        }

        public void Write(T? value, IMsgPackWriter writer, MsgPackContext context)
        {
            if (value.HasValue)
            {
                var valueConverter = context.GetConverter<T>();
                valueConverter.Write(value.Value, writer, context);
            }
            else
            {
                context.NullConverter.Write(null, writer, context);
            }
        }
    }
}