using System;
using System.Collections.Generic;

using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    internal class IndexConverter : IMsgPackConverter<Index>
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<IndexType> _indexTypeConverter;
        private IMsgPackConverter<IndexCreationOptions> _optionsConverter;
        private IMsgPackConverter<List<IndexPart>> _indexPartsConverter;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _stringConverter = context.GetConverter<string>();
            _indexTypeConverter = context.GetConverter<IndexType>();
            _optionsConverter = context.GetConverter<IndexCreationOptions>();
            _indexPartsConverter = context.GetConverter<List<IndexPart>>();
        }

        public void Write(Index value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public Index Read(IMsgPackReader reader)
        {
            var length = reader.ReadArrayLength();

            if (length != 6u)
            {
                throw ExceptionHelper.InvalidArrayLength(6u, length);
            }
        
            var spaceId = _uintConverter.Read(reader);
            var id= _uintConverter.Read(reader);
            var name = _stringConverter.Read(reader);
            var type = _indexTypeConverter.Read(reader);
            var options = _optionsConverter.Read(reader);
            var indexParts = _indexPartsConverter.Read(reader);

            return new Index(id, spaceId, name, options.Unique, type, indexParts.AsReadOnly());
        }
    }
}