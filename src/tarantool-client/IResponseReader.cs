using System.Threading.Tasks;

namespace tarantool_client
{
    public interface IResponseReader
    {
        Task BeginReading();
    }
}