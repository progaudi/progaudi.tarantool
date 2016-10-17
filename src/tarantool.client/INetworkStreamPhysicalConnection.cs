using System;
using System.Threading.Tasks;
using Tarantool.Client.Model;

namespace Tarantool.Client
{
    public interface INetworkStreamPhysicalConnection : IDisposable
    {
        void Connect(ConnectionOptions options);
        Task Flush();
        Task<int> ReadAsync(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
    }
}