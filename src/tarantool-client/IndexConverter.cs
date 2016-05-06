using System;
using System.Collections.Generic;
using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class IndexConverter : IMsgPackConverter<Index>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context= context;
        }

        public void Write(Index value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public Index Read(IMsgPackReader reader)
        {
            reader.ReadArrayLength().ShouldBe(6u);
            var uintConverter = _context.GetConverter<uint>();
            var stringConverter = _context.GetConverter<string>();
            var indexTypeConverter = _context.GetConverter<IndexType>();
            var optionsConverter = _context.GetConverter<IndexCreationOptions>();
            var indexPartsConverter = _context.GetConverter<List<IndexPart>>();

            var spaceId = uintConverter.Read(reader);
            var id= uintConverter.Read(reader);
            var name = stringConverter.Read(reader);
            var type = indexTypeConverter.Read(reader);
            var options = optionsConverter.Read(reader);
            var indexParts = indexPartsConverter.Read(reader);

            return new Index(id, spaceId, name, options.Unique, type, indexParts.AsReadOnly());
        }
    }
}