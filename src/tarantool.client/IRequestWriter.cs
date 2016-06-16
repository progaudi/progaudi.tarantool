using System.Threading.Tasks;

using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public interface IRequestWriter
    {
        Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequestPacket;

        void CompleteRequest(ulong requestId, byte[] responseBytes);
    }
}