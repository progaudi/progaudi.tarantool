using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class DeletePacketFormatter<T> : IMsgPackFormatter<DeleteRequest<T>>
    {
        private readonly IMsgPackFormatter<Key> _keyFormatter;
        private readonly IMsgPackFormatter<uint> _uintFormatter;
        private readonly IMsgPackFormatter<T> _selectKeyFormatter;

        public DeletePacketFormatter(MsgPackContext context)
        {
            _keyFormatter = context.GetRequiredFormatter<Key>();
            _uintFormatter = context.GetRequiredFormatter<uint>();
            _selectKeyFormatter = context.GetRequiredFormatter<T>();
        }

        public int GetBufferSize(DeleteRequest<T> value)
        {
            return 5 * DataLengths.UInt32 + _selectKeyFormatter.GetBufferSize(value.Key);
        }

        public bool HasConstantSize => false;
        
        public int Format(Span<byte> destination, DeleteRequest<T> value)
        {
            var result = MsgPackSpec.WriteFixMapHeader(destination, 2);

            result += _keyFormatter.Format(destination.Slice(result), Key.SpaceId);
            result += _uintFormatter.Format(destination.Slice(result), value.SpaceId);

            result += _keyFormatter.Format(destination.Slice(result), Key.IndexId);
            result += _uintFormatter.Format(destination.Slice(result), value.IndexId);

            result += _keyFormatter.Format(destination.Slice(result), Key.Key);
            result += _selectKeyFormatter.Format(destination.Slice(result), value.Key);

            return result;
        }
    }
}