namespace ProGaudi.Tarantool.Client.Model
{
    public class InsertRequest<T> : InsertReplaceRequest<T>
    {
        public InsertRequest(uint spaceId, T tuple)
            : base(CommandCodes.Insert, spaceId, tuple)
        {
        }
    }
}