using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class DeleteRequest<T> : Request
    {
        private readonly IMsgPackFormatter<DeleteRequest<T>> _formatter;

        public DeleteRequest(uint spaceId, uint indexId, T key, MsgPackContext context)
            : base(CommandCode.Delete, context)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            _formatter = context.GetRequiredFormatter<DeleteRequest<T>>();
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public T Key { get; }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}