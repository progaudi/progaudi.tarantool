using System;
using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    public class RequestHeader : HeaderBase
    {
        public RequestHeader(CommandCodes code, RequestId requestId)
            : base(code, requestId)
        {
        }

        public sealed class Formatter : IMsgPackConverter<RequestHeader>
        {
            private IMsgPackConverter<uint> _uintConverter;
            private IMsgPackConverter<ulong> _ulongConverter;
            private IMsgPackConverter<object> _nullConverter;

            public void Initialize(MsgPackContext context)
            {
                _uintConverter = context.GetConverter<uint>();
                _ulongConverter = context.GetConverter<ulong>();
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

                _uintConverter.Write(Keys.Code, writer);
                _uintConverter.Write((uint)value.Code, writer);

                _uintConverter.Write(Keys.Sync, writer);
                _ulongConverter.Write(value.RequestId.Value, writer);
            }

            public RequestHeader Read(IMsgPackReader reader)
            {
                throw new NotImplementedException();
            }
        }
    }
}