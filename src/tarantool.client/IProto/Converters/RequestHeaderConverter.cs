using System;

using MsgPack.Light;

using Shouldly;

using Tarantool.Client.IProto.Data;

namespace Tarantool.Client.IProto.Converters
{
    public class RequestHeaderConverter : IMsgPackConverter<RequestHeader>
    {
        private IMsgPackConverter<Key> _keyConverter;
        private IMsgPackConverter<ulong> _ulongConverter;
        private IMsgPackConverter<CommandCode> _codeConverter;
        private IMsgPackConverter<object> _nullConverter;

        public void Initialize(MsgPackContext context)
        {
            _keyConverter = context.GetConverter<Key>();
            _ulongConverter = context.GetConverter<ulong>();
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
            _ulongConverter.Write(value.RequestId, writer);
        }

        public RequestHeader Read(IMsgPackReader reader)
        {
            throw new NotImplementedException();
        }
    }
}