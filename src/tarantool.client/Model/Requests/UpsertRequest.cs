using Tarantool.Client.Model.Enums;
using Tarantool.Client.Model.UpdateOperations;

namespace Tarantool.Client.Model.Requests
{
    public class UpsertRequest<TTuple> : IRequest
        where TTuple : ITuple
    {
        public UpsertRequest(uint spaceId, TTuple tuple, UpdateOperation[] updateOperations)
        {
            SpaceId = spaceId;
            Tuple = tuple;
            UpdateOperations = updateOperations;
        }

        public UpdateOperation[] UpdateOperations { get; }

        public uint SpaceId { get; }

        public TTuple Tuple { get; }

        public CommandCode Code => CommandCode.Upsert;
    }
}