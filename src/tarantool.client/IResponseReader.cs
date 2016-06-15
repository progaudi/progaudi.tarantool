using System.Threading.Tasks;

namespace Tarantool.Client
{
    public interface IResponseReader
    {
        Task BeginReading();
    }
}