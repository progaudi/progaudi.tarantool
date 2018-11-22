using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class ReplaceRequest<T> : InsertReplaceRequest<T>
    {
        private readonly IMsgPackFormatter<ReplaceRequest<T>> _formatter;

        public ReplaceRequest(uint spaceId, T tuple, MsgPackContext context)
            : base(CommandCode.Replace, spaceId, tuple, context)
        {
            _formatter = context.GetRequiredFormatter<ReplaceRequest<T>>();
        }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}