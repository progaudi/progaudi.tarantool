namespace ProGaudi.Tarantool.Client.Model
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

        public CommandCodes Code => CommandCodes.Upsert;
    }
}