using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class SelectPacketFormatter<T> : IMsgPackFormatter<SelectRequest<T>>
    {
        private readonly IMsgPackFormatter<T> _selectKeyConverter;
        private readonly IMsgPackFormatter<Key> _keyConverter;
        private readonly IMsgPackFormatter<Iterator> _iteratorConverter;

        public SelectPacketFormatter(MsgPackContext context)
        {
            _keyConverter = context.GetRequiredFormatter<Key>();
            _iteratorConverter = context.GetRequiredFormatter<Iterator>();
            _selectKeyConverter = context.GetRequiredFormatter<T>();
        }

        public int GetBufferSize(SelectRequest<T> value) => 11 * DataLengths.UInt32 + _selectKeyConverter.GetBufferSize(value.SelectKey);

        public bool HasConstantSize => false;
        
        public int Format(Span<byte> destination, SelectRequest<T> value)
        {
            var result = MsgPackSpec.WriteFixMapHeader(destination, 6);

            result += _keyConverter.Format(destination.Slice(result), Key.SpaceId);
            result += MsgPackSpec.WriteUInt32(destination.Slice(result), value.SpaceId);

            result += _keyConverter.Format(destination.Slice(result), Key.IndexId);
            result += MsgPackSpec.WriteUInt32(destination.Slice(result), value.IndexId);

            result += _keyConverter.Format(destination.Slice(result), Key.Limit);
            result += MsgPackSpec.WriteUInt32(destination.Slice(result), value.Limit);

            result += _keyConverter.Format(destination.Slice(result), Key.Offset);
            result += MsgPackSpec.WriteUInt32(destination.Slice(result), value.Offset);
            
            result += _keyConverter.Format(destination.Slice(result), Key.Iterator);
            result += _iteratorConverter.Format(destination.Slice(result), value.Iterator);

            result += _keyConverter.Format(destination.Slice(result), Key.Key);
            result += _selectKeyConverter.Format(destination.Slice(result), value.SelectKey);

            return result;
        }
    }
}
