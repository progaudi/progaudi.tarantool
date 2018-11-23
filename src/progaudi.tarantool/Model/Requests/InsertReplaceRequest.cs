using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public abstract class InsertReplaceRequest<T> : Request
    {
        private readonly IMsgPackFormatter<InsertReplaceRequest<T>> _formatter;

        protected InsertReplaceRequest(CommandCode code, uint spaceId, T tuple, MsgPackContext context)
            : base(code, context)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            _formatter = context.GetRequiredFormatter<InsertReplaceRequest<T>>();
        }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);

        public uint SpaceId { get; }

        public T Tuple { get; }
    }
}