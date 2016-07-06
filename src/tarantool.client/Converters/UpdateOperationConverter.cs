using System;

using MsgPack.Light;

using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Converters
{
    internal class UpdateOperationConverter<T> : IMsgPackConverter<UpdateOperation<T>>, IMsgPackConverter<UpdateOperation>
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

        public void Write(UpdateOperation value, IMsgPackWriter writer)
        {
            Write((UpdateOperation<T>) value, writer);
        }

        UpdateOperation IMsgPackConverter<UpdateOperation>.Read(IMsgPackReader reader)
        {
            return Read(reader);
        }

        public UpdateOperation<T> Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}