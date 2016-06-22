using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Model.Requests
{
    public class UpsertRequest<TTuple, TUpdate> : IRequest
        where TTuple : ITuple
    {
        public UpsertRequest(uint spaceId, TTuple tuple, UpdateOperation<TUpdate> updateOperation)
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