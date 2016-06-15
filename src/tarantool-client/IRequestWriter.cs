using System.Threading.Tasks;

namespace tarantool_client
{
    public interface IRequestWriter
    {
        void EndRequest(ulong requestId, byte[] result);

        Task<byte[]> WriteRequest(byte[] request, ulong requestId);
    }
}