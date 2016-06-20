namespace Tarantool.Client
{
    public class ResponseReaderFactory : IResponseReaderFactory
    {
        public IResponseReader Create(ILogicalConnection logicalConnection, ConnectionOptions options)
        {
            return new ResponseReader(logicalConnection, options);
        }
    }
}