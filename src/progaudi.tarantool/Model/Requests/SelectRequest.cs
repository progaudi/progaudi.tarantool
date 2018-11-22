using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class SelectRequest<T> : Request
    {
        private readonly IMsgPackFormatter<SelectRequest<T>> _formatter;

        public SelectRequest(uint spaceId, uint indexId, uint limit, uint offset, Iterator iterator, T selectKey, MsgPackContext context)
            : base(CommandCode.Select, context)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Limit = limit;
            Offset = offset;
            Iterator = iterator;
            SelectKey = selectKey;
            _formatter = context.GetRequiredFormatter<SelectRequest<T>>();
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public uint Limit { get; }

        public uint Offset { get; }

        public Iterator Iterator { get; }

        public T SelectKey { get; }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}