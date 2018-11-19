using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class IndexCreationOptionsConverter : IMsgPackParser<IndexCreationOptions>
    {
        public IndexCreationOptions Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var optionsCount = MsgPackSpec.ReadMapHeader(source, out readSize);
            var unique = false;

            for (var i = 0; i < optionsCount; i++)
            {
                var key = MsgPackSpec.ReadString(source.Slice(readSize), out var temp);
                readSize += temp;
                switch (key)
                {
                    case nameof(unique):
                        unique = MsgPackSpec.ReadBoolean(source.Slice(readSize), out temp);
                        readSize += temp;
                        break;
                    default:
                        readSize += MsgPackSpec.ReadToken(source.Slice(readSize)).Length;
                        break;
                }
            }
            
            return new IndexCreationOptions(unique);
        }
    }
}