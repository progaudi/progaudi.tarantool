using System;
using System.Collections.Generic;
using MsgPack.Light;
using Shouldly;

namespace tarantool_client.Converters
{
    public class SpaceConverter : IMsgPackConverter<Space>
    {
        public void Write(Space value, IMsgPackWriter writer, MsgPackContext context)
        {
            throw new System.NotImplementedException();
        }

        public Space Read(IMsgPackReader reader, MsgPackContext context, Func<Space> creator)
        {
            reader.ReadArrayLength().ShouldBe(7u);

            var uintConverter = context.GetConverter<uint>();
            var stringConverter = context.GetConverter<string>();
            var engineConverter = context.GetConverter<StorageEngine>();
            var dictConverter = context.GetConverter<Dictionary<object, object>>();
            var fieldConverter = context.GetConverter<List<SpaceField>>();

            var id = uintConverter.Read(reader, context, null);

            //TODO find out what that number means
            var flag = uintConverter.Read(reader, context, null);

            var name = stringConverter.Read(reader, context, null);
            var engine = engineConverter.Read(reader, context, null);
            var fieldCount = uintConverter.Read(reader, context, null);

            //TODO find what is that dict used for 
            var dict = dictConverter.Read(reader, context, null);

            var fields = fieldConverter.Read(reader, context, null);

            return new Space(id, fieldCount, name, null, engine, fields.AsReadOnly());
        }
    }
}