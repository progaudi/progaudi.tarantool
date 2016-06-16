using System.Threading.Tasks;

namespace Tarantool.Client
{
    public interface IRequestQueue
    {
        void Dequeue(ulong requestId, byte[] responseBytes);

        Task<byte[]> Queue(ulong requestId);
    }
}