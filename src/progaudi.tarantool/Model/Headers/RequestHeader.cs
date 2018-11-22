using System;
using System.Threading;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Headers
{
    public class RequestHeader : HeaderBase
    {
        private static long _currentRequestId;
        private readonly IMsgPackFormatter<RequestHeader> _formatter;

        public RequestHeader(CommandCode code, MsgPackContext context)
            : base(code, (RequestId) (ulong) Interlocked.Increment(ref _currentRequestId))
        {
            _formatter = context.GetRequiredFormatter<RequestHeader>();
        }

        public int WriteTo(in Span<byte> buffer) => _formatter.Format(buffer, this);

        public int GetApproximateLength() => Constants.MaxHeaderLength;
    }
}