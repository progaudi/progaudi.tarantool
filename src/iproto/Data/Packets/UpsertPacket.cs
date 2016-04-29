using iproto.Data.UpdateOperations;

namespace iproto.Data.Packets
{
    public class UpsertPacket<TTuple, TUpdate> : UnifiedPacket
        where TTuple : IMyTuple
    {
        public UpsertPacket(uint spaceId, TTuple tuple, UpdateOperation<TUpdate> updateOperation)
            : base(new Header(CommandCode.Upsert, null, null))
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public uint SpaceId { get; }

        public TTuple Tuple { get; }
    }
}