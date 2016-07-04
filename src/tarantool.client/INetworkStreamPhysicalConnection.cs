using System;
using System.Threading.Tasks;
using Tarantool.Client.Model;

namespace Tarantool.Client
{
    public interface INetworkStreamPhysicalConnection : IDisposable
    {
        IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);
        void Connect(ConnectionOptions options);
        int EndRead(IAsyncResult asyncResult);
        Task Flush();
        Task<int> Read(byte[] buffer, int offset, int count);
        void Write(byte[] buffer, int offset, int count);
    }
}