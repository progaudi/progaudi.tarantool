using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Converters
{
    public class SpaceFieldParser : IMsgPackParser<SpaceField>
    {
        private readonly IMsgPackParser<FieldType> _typeConverter;

        public SpaceFieldParser(MsgPackContext context)
        {
            _typeConverter = context.GetRequiredParser<FieldType>();
        }

        public SpaceField Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);
            string name = default;
            var type = (FieldType) (-1);
            
            for (var i = 0; i < length; i++)
            {
                var key = MsgPackSpec.ReadString(source.Slice(readSize), out var temp); readSize += temp; 
                switch (key)
                {
                    case "name":
                        name = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp; 
                        break;
                    case "type":
                        type = _typeConverter.Parse(source.Slice(readSize), out temp); readSize += temp; 
                        break;
                    default:
                        readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).Length;
                        break;
                }
            }

            return new SpaceField(name, type);
        }
    }
}