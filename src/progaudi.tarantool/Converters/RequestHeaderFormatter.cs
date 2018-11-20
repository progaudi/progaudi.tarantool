using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class RequestHeaderFormatter : IMsgPackFormatter<RequestHeader>
    {
        private readonly IMsgPackFormatter<Key> _keyConverter;
        private readonly IMsgPackFormatter<CommandCode> _codeConverter;

        public RequestHeaderFormatter(MsgPackContext context)
        {
            _keyConverter = context.GetRequiredFormatter<Key>();
            _codeConverter = context.GetRequiredFormatter<CommandCode>();
        }

        public int GetBufferSize(RequestHeader value) => DataLengths.FixMapHeader + 2 * DataLengths.UInt32 + DataLengths.UInt64;

        public bool HasConstantSize => true;
        
        public int Format(Span<byte> destination, RequestHeader value)
        {
            var result = MsgPackSpec.WriteFixMapHeader(destination, 2);

            result += _keyConverter.Format(destination.Slice(result), Key.Code);
            result += _codeConverter.Format(destination.Slice(result), value.Code);
            result += _keyConverter.Format(destination.Slice(result), Key.Sync);
            result += MsgPackSpec.WriteFixUInt64(destination.Slice(result), value.RequestId.Value);

            return result;
        }
    }
}