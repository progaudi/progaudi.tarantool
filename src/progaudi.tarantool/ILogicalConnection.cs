using System;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;

namespace ProGaudi.Tarantool.Client
{
    public interface ILogicalConnection : IDisposable
    {
        Task Connect();

        bool IsConnected();

        Task SendRequestWithEmptyResponse<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest;

        Task<DataResponse<TResponse[]>> SendRequest<TRequest, TResponse>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest;

        Task<byte[]> SendRawRequest<TRequest>(TRequest request, TimeSpan? timeout = null)
            where TRequest : IRequest;

        uint PingsFailedByTimeoutCount { get; }
    }
}