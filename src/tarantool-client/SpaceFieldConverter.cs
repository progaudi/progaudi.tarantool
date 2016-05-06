using System;
using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class SpaceFieldConverter:IMsgPackConverter<SpaceField>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context= context;
        }

        public void Write(SpaceField value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public SpaceField Read(IMsgPackReader reader)
        {
            var dictLength = reader.ReadMapLength();
            var stringConverter = _context.GetConverter<string>();
            var typeConverter = _context.GetConverter<FieldType>();

            string name = null;
            var type = (FieldType) (-1);

            for (int i = 0; i < dictLength.Value; i++)
            {
                var key = stringConverter.Read(reader);
                switch (key)
                {
                    case "name":
                        name = stringConverter.Read(reader);
                        break;
                    case "type":
                        type = typeConverter.Read(reader);
                        break;
                    default:
                        throw new ArgumentException($"Invalid SpaceField key: {key}");
                }
            }

            return new SpaceField(name, type);
        }
    }
}