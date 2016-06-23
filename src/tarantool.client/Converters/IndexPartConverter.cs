using MsgPack.Light;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Enums;
using Tarantool.Client.Utils;

namespace Tarantool.Client.Converters
{
    internal class IndexPartConverter : IMsgPackConverter<IndexPart>
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
            var length = reader.ReadArrayLength();
            if (length != 2u)
            {
                throw ExceptionHelper.InvalidArrayLength(2u, length);
            }


            var fieldNo = _uintConverter.Read(reader);
            var indexPartType = _indexPartTypeConverter.Read(reader);

            return new IndexPart(fieldNo, indexPartType);
        }
    }
}