using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class PingRequest : Request
    {
        public PingRequest(MsgPackContext context) : base(CommandCode.Ping, context)
        {
        }

        protected override int GetApproximateBodyLength() => 0;

        protected override int WriteBodyTo(Span<byte> buffer) => 0;
    }
}