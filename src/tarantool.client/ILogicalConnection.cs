using System.Threading.Tasks;

using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public interface ILogicalConnection
    {
        Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequestPacket;

        TaskCompletionSource<byte[]> GetResponseCompletionSource(ulong requestId);
    }
}