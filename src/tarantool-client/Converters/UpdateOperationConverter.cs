using System;

using iproto.Data.UpdateOperations;

using TarantoolDnx.MsgPack;

namespace tarantool_client.Converters
{
    public class UpdateOperationConverter<T> : IMsgPackConverter<UpdateOperation<T>>
    {
        public void Write(UpdateOperation<T> value, IMsgPackWriter writer, MsgPackContext context)
        {
            var stringConverter = context.GetConverter<string>();
            var intConverter = context.GetConverter<int>();
            var argumentConverter = context.GetConverter<T>();

            writer.WriteArrayHeader(3);

            stringConverter.Write(value.OperationType, writer, context);
            intConverter.Write(value.FieldNumber, writer, context);
            argumentConverter.Write(value.Argument, writer, context);
        }

        public UpdateOperation<T> Read(IMsgPackReader reader, MsgPackContext context, Func<UpdateOperation<T>> creator)
        {
            throw new NotImplementedException();
        }
    }
}