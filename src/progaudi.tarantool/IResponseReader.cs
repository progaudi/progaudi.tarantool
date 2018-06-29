using System;
using System.IO;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IResponseReader : IDisposable
    {
        void BeginReading();

        Result<TResponse> GetResponseTask<TResponse>(RequestId requestId, Action<MemoryStream, Result<TResponse>> responseCreator);

        bool IsConnected { get; }
    }
}