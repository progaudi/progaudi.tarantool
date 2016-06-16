using System.Threading.Tasks;

using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public interface IConnection
    {
        Task<TResponse> SendPacket<TRequest, TResponse>(TRequest request)
            where TRequest : IRequestPacket;
    }
}