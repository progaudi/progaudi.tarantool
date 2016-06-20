namespace Tarantool.Client
{
    public interface IResponseReaderFactory
    {
        IResponseReader Create(ILogicalConnection logicalConnection, ConnectionOptions options);
    }
}