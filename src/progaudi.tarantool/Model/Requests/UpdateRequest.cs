using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class UpdateRequest<TKey> : Request
    {
        private readonly IMsgPackFormatter<UpdateRequest<TKey>> _formatter;

        public UpdateRequest(uint spaceId, uint indexId, TKey key, MsgPackContext context, params UpdateOperation[] updateOperations)
            : base(CommandCode.Update, context)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperations = updateOperations;
            _formatter = context.GetRequiredFormatter<UpdateRequest<TKey>>();
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public TKey Key { get; }

        public UpdateOperation[] UpdateOperations { get; }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}