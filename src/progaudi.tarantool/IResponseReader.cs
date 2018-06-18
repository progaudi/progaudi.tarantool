using System;
using System.IO;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IResponseReader : IDisposable
    {
        void BeginReading();

        Task<TResponse> GetResponseTask<TResponse>(RequestId requestId, Func<MemoryStream, TResponse> responseCreator);

        bool IsConnected { get; }
    }
}