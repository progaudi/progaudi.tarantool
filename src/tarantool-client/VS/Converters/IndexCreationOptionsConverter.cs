using System;
using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class IndexCreationOptionsConverter:IMsgPackConverter<IndexCreationOptions>
    {
        public void Write(IndexCreationOptions value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new System.NotImplementedException();
        }

        public IndexCreationOptions Read(IMsgPackReader reader, MsgPackContext context, Func<IndexCreationOptions> creator)
        {
            var optionsCount = reader.ReadMapLength();
            var stringConverter = context.GetConverter<string>();
            var boolConverter = context.GetConverter<bool>();

            var unique = false;
            for (int i = 0; i < optionsCount.Value; i++)
            {
                var key = stringConverter.Read(reader, context, null);
                switch (key)
                {
                    case "unique":
                        unique = boolConverter.Read(reader, context, null);
                        break;
                    default:
                        throw new ArgumentException($"Unknown index creation option: {key}");
                }
            }

            return new IndexCreationOptions(unique);
        }
    }
}