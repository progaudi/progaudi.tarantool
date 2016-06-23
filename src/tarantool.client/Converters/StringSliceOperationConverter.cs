using System;

using MsgPack.Light;

using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Converters
{
    internal class StringSliceOperationConverter : IMsgPackConverter<StringSliceOperation>
    {
        private IMsgPackConverter<string> _stringConverter;
        private IMsgPackConverter<int> _intConverter;

        public void Initialize(MsgPackContext context)
        {
            _stringConverter = context.GetConverter<string>();
            _intConverter = context.GetConverter<int>();
        }

        public void Write(StringSliceOperation value, IMsgPackWriter writer)
        {
        
            writer.WriteArrayHeader(5);

            _stringConverter.Write(value.OperationType, writer);
            _intConverter.Write(value.FieldNumber, writer);
            _intConverter.Write(value.Position, writer);
            _intConverter.Write(value.Offset, writer);
            _stringConverter.Write(value.Argument, writer);
        }

        public StringSliceOperation Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}