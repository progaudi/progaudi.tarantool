using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class InsertReplacePacketFormatter<T> : IMsgPackFormatter<InsertReplaceRequest<T>>
    {
        private readonly IMsgPackFormatter<Key> _keyConverter;
        private readonly IMsgPackFormatter<T> _tupleConverter;

        public InsertReplacePacketFormatter(MsgPackContext context)
        {
            _keyConverter = context.GetRequiredFormatter<Key>();
            _tupleConverter = context.GetRequiredFormatter<T>();
        }

        public int GetBufferSize(InsertReplaceRequest<T> value) => DataLengths.FixMapHeader
                                                                   + 3 * DataLengths.UInt32 
                                                                   + _tupleConverter.GetBufferSize(value.Tuple);

        public bool HasConstantSize => false;
        
        public int Format(Span<byte> destination, InsertReplaceRequest<T> value)
        {
            var result = MsgPackSpec.WriteFixMapHeader(destination, 2);
            
            result += _keyConverter.Format(destination.Slice(result), Key.SpaceId);
            result += MsgPackSpec.WriteUInt32(destination.Slice(result), value.SpaceId);
            result += _keyConverter.Format(destination.Slice(result), Key.Tuple);
            result += _tupleConverter.Format(destination.Slice(result), value.Tuple);

            return result;
        }
    }
}