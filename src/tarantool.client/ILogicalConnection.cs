using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Tarantool.Client.IProto.Data;
using Tarantool.Client.IProto.Data.Packets;

namespace Tarantool.Client
{
    public interface ILogicalConnection
    {
        Task<TResponse> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequestPacket;

        TaskCompletionSource<MemoryStream> PopResponseCompletionSource(RequestId requestId);

        IEnumerable<TaskCompletionSource<MemoryStream>> PopAllResponseCompletionSources();
    }
}