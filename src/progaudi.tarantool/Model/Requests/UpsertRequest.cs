using System;
using ProGaudi.MsgPack;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class UpsertRequest<TTuple> : Request
    {
        private readonly IMsgPackFormatter<UpsertRequest<TTuple>> _formatter;

        public UpsertRequest(uint spaceId, TTuple tuple, MsgPackContext context, params UpdateOperation[] updateOperations)
            : base(CommandCode.Upsert, context)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperations = updateOperations;
            _formatter = context.GetRequiredFormatter<UpsertRequest<TTuple>>();
        }

        public UpdateOperation[] UpdateOperations { get; }

        public uint SpaceId { get; }

        public TTuple Tuple { get; }

        protected override int GetApproximateBodyLength() => _formatter.GetBufferSize(this);

        protected override int WriteBodyTo(Span<byte> buffer) => _formatter.Format(buffer, this);
    }
}