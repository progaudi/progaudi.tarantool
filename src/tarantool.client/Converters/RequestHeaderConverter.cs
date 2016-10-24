using System;

using ProGaudi.MsgPack.Light;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;

namespace ProGaudi.Tarantool.Client.Converters
{
    internal class RequestHeaderConverter : IMsgPackConverter<RequestHeader>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<RequestId> _requestIdConverter;
        private IMsgPackConverter<CommandCode> _codeConverter;
        private IMsgPackConverter<object> _nullConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _requestIdConverter = context.GetConverter<RequestId>();
            _codeConverter = context.GetConverter<CommandCode>();
            _nullConverter = context.NullConverter;
        }

        public void Write(RequestHeader value, IMsgPackWriter writer)
        {
            if (value == null)
            {
                _nullConverter.Write(null, writer);
                return;
            }

            writer.WriteMapHeader(2u);

            _keyConverter.Write(Key.Code, writer);
            _codeConverter.Write(value.Code, writer);

            _keyConverter.Write(Key.Sync, writer);
            _requestIdConverter.Write(value.RequestId, writer);
        }

        public RequestHeader Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}