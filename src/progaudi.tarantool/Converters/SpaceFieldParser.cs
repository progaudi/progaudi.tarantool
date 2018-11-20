using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Converters
{
    public class SpaceFieldParser : IMsgPackParser<SpaceField>
    {
        public SpaceField Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadMapHeader(source, out readSize);
            string name = default;
            string type = default;
            
            for (var i = 0; i < length; i++)
            {
                var key = MsgPackSpec.ReadString(source.Slice(readSize), out var temp); readSize += temp; 
                switch (key)
                {
                    case "name":
                        name = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp; 
                        break;
                    case "type":
                        type = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp; 
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