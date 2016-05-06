using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class IndexPartConverter : IMsgPackConverter<IndexPart>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(IndexPart value, IMsgPackWriter writer)
        {
            throw new System.NotImplementedException();
        }

        public IndexPart Read(IMsgPackReader reader)
        {
            reader.ReadArrayLength().ShouldBe(2u);

            var uintConverter = _context.GetConverter<uint>();
            var indexPartTypeConverter = _context.GetConverter<IndexPartType>();

            var fieldNo = uintConverter.Read(reader);
            var indexPartType = indexPartTypeConverter.Read(reader);

            return new IndexPart(fieldNo, indexPartType);
        }
    }
}