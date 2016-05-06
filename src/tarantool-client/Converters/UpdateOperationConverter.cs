using System;

using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class UpdateOperationConverter<T> : IMsgPackConverter<UpdateOperation<T>>
    {
        private MsgPackContext _context;

        public void Initialize(MsgPackContext context)
        {
            _context = context;
        }

        public void Write(UpdateOperation<T> value, IMsgPackWriter writer)
        {
            var stringConverter = _context.GetConverter<string>();
            var intConverter = _context.GetConverter<int>();
            var argumentConverter = _context.GetConverter<T>();

            writer.WriteArrayHeader(3);

            stringConverter.Write(value.OperationType, writer);
            intConverter.Write(value.FieldNumber, writer);
            argumentConverter.Write(value.Argument, writer);
        }

        public UpdateOperation<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}