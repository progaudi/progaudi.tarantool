namespace ProGaudi.Tarantool.Client.Model
{
    public class ReplaceRequest<T> : InsertReplaceRequest<T>
    {
        public ReplaceRequest(uint spaceId, T tuple)
            : base(CommandCodes.Replace, spaceId, tuple)
        {
        }
    }
}