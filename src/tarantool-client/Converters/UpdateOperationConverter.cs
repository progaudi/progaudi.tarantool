using System;

using iproto.Data.UpdateOperations;

using MsgPack.Light;

namespace tarantool_client.Converters
{
    public class UpdateOperationConverter<T> : IMsgPackConverter<UpdateOperation<T>>
    {
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<int> _intConverter;
        private IMsgPackConverter<T> _argumentConverter;

        public void Initialize(MsgPackContext context)
        {
            _stringConverter = context.GetConverter<string>();
            _intConverter = context.GetConverter<int>();
            _argumentConverter = context.GetConverter<T>();
        }

        public void Write(UpdateOperation<T> value, IMsgPackWriter writer)
        {
        
            writer.WriteArrayHeader(3);

            _stringConverter.Write(value.OperationType, writer);
            _intConverter.Write(value.FieldNumber, writer);
            _argumentConverter.Write(value.Argument, writer);
        }

        public UpdateOperation<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}