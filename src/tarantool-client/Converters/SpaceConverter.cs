using System;
using System.Collections.Generic;
using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class SpaceConverter : IMsgPackConverter<Space>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(Space value, IMsgPackWriter writer)
        {
            throw new NotImplementedException();
        }

        public Space Read(IMsgPackReader reader)
        {
            reader.ReadArrayLength().ShouldBe(7u);

            var uintConverter = _context.GetConverter<uint>();
            var stringConverter = _context.GetConverter<string>();
            var engineConverter = _context.GetConverter<StorageEngine>();
            var dictConverter = _context.GetConverter<Dictionary<object, object>>();
            var fieldConverter = _context.GetConverter<List<SpaceField>>();

            var id = uintConverter.Read(reader);

            //TODO find out what that number means
            uintConverter.Read(reader);

            var name = stringConverter.Read(reader);
            var engine = engineConverter.Read(reader);
            var fieldCount = uintConverter.Read(reader);

            //TODO find what is that dict used for 
            dictConverter.Read(reader);

            var fields = fieldConverter.Read(reader);

            return new Space(id, fieldCount, name, null, engine, fields.AsReadOnly(), null);
        }
    }
}