using System;
using System.Collections.Generic;
using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class IndexConverter : IMsgPackConverter<Index>
    {
        public void Write(Index value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new System.NotImplementedException();
        }

        public Index Read(IMsgPackReader reader, MsgPackContext context, Func<Index> creator)
        {
            reader.ReadArrayLength().ShouldBe(6u);
            var uintConverter = context.GetConverter<uint>();
            var stringConverter = context.GetConverter<string>();
            var indexTypeConverter = context.GetConverter<IndexType>();
            var optionsConverter = context.GetConverter<IndexCreationOptions>();
            var indexPartsConverter = context.GetConverter<List<IndexPart>>();

            var spaceId = uintConverter.Read(reader, context, null);
            var id= uintConverter.Read(reader, context, null);
            var name = stringConverter.Read(reader, context, null);
            var type = indexTypeConverter.Read(reader, context, null);
            var options = optionsConverter.Read(reader, context, null);
            var indexParts = indexPartsConverter.Read(reader, context, null);

            return new Index(id, spaceId, name, options.Unique, type, indexParts.AsReadOnly());
        }
    }
}