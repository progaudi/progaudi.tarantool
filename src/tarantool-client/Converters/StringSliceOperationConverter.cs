using System;

using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class StringSliceOperationConverter : IMsgPackConverter<StringSliceOperation>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(StringSliceOperation value, IMsgPackWriter writer)
        {
            var stringConverter = _context.GetConverter<string>();
            var intConverter = _context.GetConverter<int>();

            writer.WriteArrayHeader(5);

            stringConverter.Write(value.OperationType, writer);
            intConverter.Write(value.FieldNumber, writer);
            intConverter.Write(value.Position, writer);
            intConverter.Write(value.Offset, writer);
            stringConverter.Write(value.Argument, writer);
        }

        public StringSliceOperation Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}