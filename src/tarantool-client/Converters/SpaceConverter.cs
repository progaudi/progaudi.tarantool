using System;
using System.Collections.Generic;
using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class SpaceConverter : IMsgPackConverter<Space>
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<StorageEngine> _engineConverter;
        private IMsgPackConverter<Dictionary<object, object>> _dictConverter;
        private IMsgPackConverter<List<SpaceField>> _fieldConverter;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _stringConverter = context.GetConverter<string>();
            _engineConverter = context.GetConverter<StorageEngine>();
            _dictConverter = context.GetConverter<Dictionary<object, object>>();
            _fieldConverter = context.GetConverter<List<SpaceField>>();
        }

        public void Write(Space value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public Space Read(IMsgPackReader reader)
        {
            reader.ReadArrayLength().ShouldBe(7u);

        
            var id = _uintConverter.Read(reader);

            //TODO find out what that number means
            _uintConverter.Read(reader);

            var name = _stringConverter.Read(reader);
            var engine = _engineConverter.Read(reader);
            var fieldCount = _uintConverter.Read(reader);

            //TODO find what is that dict used for 
            _dictConverter.Read(reader);

            var fields = _fieldConverter.Read(reader);

            return new Space(id, fieldCount, name, null, engine, fields.AsReadOnly(), null);
        }
    }
}