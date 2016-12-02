using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;

namespace ProGaudi.Tarantool.Client
{
    using System;

    public interface ILogicalConnection : IDisposable
    {
        Func<GreetingsResponse, Task> GreetingFunc { get; set; }

        Task Connect();
        
        Task SendRequestWithEmptyResponse<TRequest>(TRequest request)
            where TRequest : IRequest;

        Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request)
            where TRequest : IRequest;

        TaskCompletionSource<MemoryStream> PopResponseCompletionSource(RequestId requestId, MemoryStream resultStream);

        IEnumerable<TaskCompletionSource<MemoryStream>> PopAllResponseCompletionSources();

        void CancelAllPendingRequests();
    }
}