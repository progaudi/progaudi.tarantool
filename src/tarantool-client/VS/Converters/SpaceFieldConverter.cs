using System;
using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class SpaceFieldConverter:IMsgPackConverter<SpaceField>
    {
        public void Write(SpaceField value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new System.NotImplementedException();
        }

        public SpaceField Read(IMsgPackReader reader, MsgPackContext context, Func<SpaceField> creator)
        {
            var dictLength = reader.ReadMapLength();
            var stringConverter = context.GetConverter<string>();
            var typeConverter = context.GetConverter<FieldType>();

            string name = null;
            var type = (FieldType) (-1);

            for (int i = 0; i < dictLength.Value; i++)
            {
                var key = stringConverter.Read(reader, context, null);
                switch (key)
                {
                    case "name":
                        name = stringConverter.Read(reader, context, null);
                        break;
                    case "type":
                        type = typeConverter.Read(reader, context, null);
                        break;
                    default:
                        throw new ArgumentException($"Invalid SpaceField key: {key}");
                }
            }

            return new SpaceField(name, type);
        }
    }
}