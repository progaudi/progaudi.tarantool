using System;
using System.Collections.Generic;

using MsgPack.Light;

using Shouldly;

namespace Tarantool.Client.IProto.Converters
{
    public class SpaceConverter : IMsgPackConverter<Space>
    {
        private IMsgPackConverter<uint> _uintConverter;
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<StorageEngine> _engineConverter;
        private IMsgPackConverter<List<SpaceField>> _fieldConverter;

        public void Initialize(MsgPackContext context)
        {
            _uintConverter = context.GetConverter<uint>();
            _stringConverter = context.GetConverter<string>();
            _engineConverter = context.GetConverter<StorageEngine>();
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

            //TODO Find what skipped number means
            reader.SkipToken();

            var name = _stringConverter.Read(reader);
            var engine = _engineConverter.Read(reader);
            var fieldCount = _uintConverter.Read(reader);

            //TODO Find what skipped dictionary used for
            reader.SkipToken();

            var fields = _fieldConverter.Read(reader);

            return new Space(id, fieldCount, name, null, engine, fields.AsReadOnly());
        }
    }
}