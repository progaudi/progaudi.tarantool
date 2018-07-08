namespace ProGaudi.Tarantool.Client.Model
{
    public class UpdateRequest<TKey> : IRequest
    {
        public UpdateRequest(uint spaceId, uint indexId, TKey key, UpdateOperation[] updateOperations)
        {
            SpaceId = spaceId;
            IndexId = indexId;
            Key = key;
            UpdateOperations = updateOperations;
        }

        public uint SpaceId { get; }

        public uint IndexId { get; }

        public TKey Key { get; }

        public UpdateOperation[] UpdateOperations { get; }

        public CommandCodes Code => CommandCodes.Update;
    }
}