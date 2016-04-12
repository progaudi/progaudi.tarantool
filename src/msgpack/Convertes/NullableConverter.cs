using System;

namespace TarantoolDnx.MsgPack.Convertes
{
    public class NullableConverter<T> : IMsgPackConverter<T?> where T : struct
    {
        public T? Read(IMsgPackReader reader, MsgPackContext context, Func<T?> creator)
        {
            var type = reader.ReadDataType();
            if (type == DataTypes.Null)
                return null;

            var structConverter= context.GetStructConverter<T>();

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
            return structConverter.ReadWithoutTypeReading(type, reader, context, nullableCreator);
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