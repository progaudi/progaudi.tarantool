using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class IndexPartConverter : IMsgPackConverter<IndexPart>
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<IndexPartType> _indexPartTypeConverter;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _indexPartTypeConverter = context.GetConverter<IndexPartType>();
        }

        public void Write(IndexPart value, IMsgPackWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public IndexPart Read(IMsgPackReader reader)
        {
            reader.ReadArrayLength().ShouldBe(2u);

        
            var fieldNo = _uintConverter.Read(reader);
            var indexPartType = _indexPartTypeConverter.Read(reader);

            return new IndexPart(fieldNo, indexPartType);
        }
    }
}