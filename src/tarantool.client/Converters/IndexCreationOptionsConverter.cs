using System;

using MsgPack.Light;

using Tarantool.Client.Model;

namespace Tarantool.Client.Converters
{
    public class IndexCreationOptionsConverter:IMsgPackConverter<IndexCreationOptions>
    {
        private MsgPackContext context;

        public void Initialize(MsgPackContext context)
        {
            this.context= context;
        }

        public void Write(IndexCreationOptions value, IMsgPackWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public IndexCreationOptions Read(IMsgPackReader reader)
        {
            var optionsCount = reader.ReadMapLength();
            var stringConverter = context.GetConverter<string>();
            var boolConverter = context.GetConverter<bool>();

            var unique = false;
            for (int i = 0; i < optionsCount.Value; i++)
            {
                var key = stringConverter.Read(reader);
                switch (key)
                {
                    case "unique":
                        unique = boolConverter.Read(reader);
                        break;
                    default:
                        throw new ArgumentException($"Unknown index creation option: {key}");
                }
            }

            return new IndexCreationOptions(unique);
        }
    }
}