using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class EvalPacketFormatter<T> : IMsgPackFormatter<EvalRequest<T>>
    {
        private readonly IMsgPackFormatter<Key> _keyFormatter;
        private readonly IMsgPackFormatter<string> _stringFormatter;
        private readonly IMsgPackFormatter<T> _tupleFormatter;

        public EvalPacketFormatter(MsgPackContext context)
        {
            _keyFormatter = context.GetRequiredFormatter<Key>();
            _stringFormatter = context.GetRequiredFormatter<string>();
            _tupleFormatter = context.GetRequiredFormatter<T>();
        }

        public int GetBufferSize(EvalRequest<T> value) => 2 * DataLengths.UInt32
                                                          + _stringFormatter.GetBufferSize(value.Expression)
                                                          + _tupleFormatter.GetBufferSize(value.Tuple);

        public bool HasConstantSize => false;

        public int Format(Span<byte> destination, EvalRequest<T> value)
        {
            var result = MsgPackSpec.WriteFixMapHeader(destination, 2);

            result += _keyFormatter.Format(destination.Slice(result), Key.Expression);
            result += _stringFormatter.Format(destination.Slice(result), value.Expression);

            result += _keyFormatter.Format(destination.Slice(result), Key.Tuple);
            result += _tupleFormatter.Format(destination.Slice(result), value.Tuple);
            
            return result;
        }
    }
}