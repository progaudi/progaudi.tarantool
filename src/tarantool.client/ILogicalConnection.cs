using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Requests;
using Tarantool.Client.Model.Responses;

namespace Tarantool.Client
{
    public interface ILogicalConnection
    {
        Task SendRequestWithEmptyResponse<TRequest>(TRequest request)
            where TRequest : IRequest;

        Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest;

        TaskCompletionSource<MemoryStream> PopResponseCompletionSource(RequestId requestId, MemoryStream resultStream);

        IEnumerable<TaskCompletionSource<MemoryStream>> PopAllResponseCompletionSources();
    }
}