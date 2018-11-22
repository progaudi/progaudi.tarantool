using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.Headers;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public abstract class Request
    {
        protected Request(CommandCode code, MsgPackContext context)
        {
            Header = new RequestHeader(code, context);
        }

        public RequestHeader Header { get; }

        public int GetApproximateLength() => Header.GetApproximateLength() + GetApproximateBodyLength();
        
        protected abstract int GetApproximateBodyLength();

        public int WriteTo(Span<byte> buffer)
        {
            var offset = Header.WriteTo(buffer);
            return offset + WriteBodyTo(buffer.Slice(offset));
        }

        protected abstract int WriteBodyTo(Span<byte> buffer);
    }
}