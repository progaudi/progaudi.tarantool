using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class UpsertRequest<TTuple> : IRequest
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