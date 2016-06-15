using System.Threading.Tasks;

namespace Tarantool.Client
{
    public interface IRequestWriter
    {
        void EndRequest(ulong requestId, byte[] result);

        Task<byte[]> WriteRequest(byte[] request, ulong requestId);
    }
}