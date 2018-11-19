using System;
using System.Collections.Generic;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class IndexConverter : IMsgPackParser<Index>
    {
        private readonly IMsgPackParser<IndexType> _indexTypeConverter;
        private readonly IMsgPackParser<IndexCreationOptions> _optionsConverter;
        private readonly IMsgPackParser<List<IndexPart>> _indexPartsConverter;

        public IndexConverter(MsgPackContext context)
        {
            _indexTypeConverter = context.GetRequiredParser<IndexType>();
            _optionsConverter = context.GetRequiredParser<IndexCreationOptions>();
            _indexPartsConverter = context.GetRequiredParser<List<IndexPart>>();
        }

        public Index Parse(ReadOnlySpan<byte> source, out int readSize)
        {
            var length = MsgPackSpec.ReadArrayHeader(source, out readSize);
            
            if (length != 6u)
            {
                throw ExceptionHelper.InvalidArrayLength(6u, length);
            }

            var spaceId = MsgPackSpec.ReadUInt32(source.Slice(readSize), out var temp); readSize += temp;
            var id= MsgPackSpec.ReadUInt32(source.Slice(readSize), out temp); readSize += temp;
            var name = MsgPackSpec.ReadString(source.Slice(readSize), out temp); readSize += temp;
            var type = _indexTypeConverter.Parse(source.Slice(readSize), out temp); readSize += temp;
            var options = _optionsConverter.Parse(source.Slice(readSize), out temp); readSize += temp;
            var indexParts = _indexPartsConverter.Parse(source.Slice(readSize), out temp); readSize += temp;

            return new Index(id, spaceId, name, options.Unique, type, indexParts.AsReadOnly());
        }
    }
}