using Tarantool.Client.IProto.Data.UpdateOperations;

namespace Tarantool.Client.IProto.Data.Packets
{
    public class UpsertPacket<TTuple, TUpdate> : IRequestPacket
        where TTuple : ITuple
    {
        public UpsertPacket(uint spaceId, TTuple tuple, UpdateOperation<TUpdate> updateOperation)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperation = updateOperation;
        }

        public UpdateOperation<TUpdate> UpdateOperation { get; }

        public uint SpaceId { get; }

        public TTuple Tuple { get; }

        public CommandCode Code => CommandCode.Upsert;
    }
}