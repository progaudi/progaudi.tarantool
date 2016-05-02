using System;

using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class StringSliceOperationConverter : IMsgPackConverter<StringSliceOperation>
    {
        public void Write(StringSliceOperation value, IMsgPackWriter writer, MsgPackContext context)
        {
            var stringConverter = context.GetConverter<string>();
            var intConverter = context.GetConverter<int>();

            writer.WriteArrayHeader(5);

            stringConverter.Write(value.OperationType, writer, context);
            intConverter.Write(value.FieldNumber, writer, context);
            intConverter.Write(value.Position, writer, context);
            intConverter.Write(value.Offset, writer, context);
            stringConverter.Write(value.Argument, writer, context);
        }

        public StringSliceOperation Read(IMsgPackReader reader, MsgPackContext context, Func<StringSliceOperation> creator)
        {
            throw new NotImplementedException();
        }
    }
}