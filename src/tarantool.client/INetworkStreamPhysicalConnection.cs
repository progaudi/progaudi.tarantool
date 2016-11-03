using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface INetworkStreamPhysicalConnection : IDisposable
    {
        Task Connect(ClientOptions options);
        Task Flush();
        Task<int> Read(byte[] buffer, int offset, int count);
        Task Write(byte[] buffer, int offset, int count);
    }
}