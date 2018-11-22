using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class InsertRequest<T> : InsertReplaceRequest<T>
    {
        private readonly IMsgPackFormatter<InsertRequest<T>> _formatter;

        public InsertRequest(uint spaceId, T tuple, MsgPackContext context)
            : base(CommandCode.Insert, spaceId, tuple, context)
        {
            _formatter = context.GetRequiredFormatter<InsertRequest<T>>();
        }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}