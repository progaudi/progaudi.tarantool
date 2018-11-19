using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class CallPacketFormatter<T> : IMsgPackFormatter<CallRequest<T>>
    {
        private readonly IMsgPackFormatter<Key> _keyFormatter;
        private readonly IMsgPackFormatter<string> _stringFormatter;
        private readonly IMsgPackFormatter<T> _tupleFormatter;

        public CallPacketFormatter(MsgPackContext context)
        {
            _keyFormatter = context.GetRequiredFormatter<Key>();
            _stringFormatter = context.GetRequiredFormatter<string>();
            _tupleFormatter = context.GetRequiredFormatter<T>();
        }

        public int GetBufferSize(CallRequest<T> value) => 2 * DataLengths.UInt32
                                                          + _stringFormatter.GetBufferSize(value.FunctionName)
                                                          + _tupleFormatter.GetBufferSize(value.Tuple);

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, CallRequest<T> value)
        {
            var result = MsgPackSpec.WriteFixMapHeader(destination, 2);

            result += _keyFormatter.Format(destination.Slice(result), Key.FunctionName);
            result += _stringFormatter.Format(destination.Slice(result), value.FunctionName);

            result += _keyFormatter.Format(destination.Slice(result), Key.FunctionName);
            result += _tupleFormatter.Format(destination.Slice(result), value.Tuple);
            
            return result;
        }
    }
}