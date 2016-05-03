using System;
using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class IndexPartConverter : IMsgPackConverter<IndexPart>
    {
        public void Write(IndexPart value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new System.NotImplementedException();
        }

        public IndexPart Read(IMsgPackReader reader, MsgPackContext context, Func<IndexPart> creator)
        {
            reader.ReadArrayLength().ShouldBe(2u);

            var uintConverter = context.GetConverter<uint>();
            var indexPartTypeConverter = context.GetConverter<IndexPartType>();

            var fieldNo = uintConverter.Read(reader, context, null);
            var indexPartType = indexPartTypeConverter.Read(reader, context, null);

            return new IndexPart(fieldNo, indexPartType);
        }
    }
}