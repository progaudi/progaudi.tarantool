using System;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    public interface IPhysicalConnection : IDisposable
    {
        void Connect(ConnectionOptions options);

        Task<int> ReadAsync(byte[] buffer, int offset, int count);

        Task WriteAsync(byte[] buffer, int offset, int count);

        Task FlushAsync();

        IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state);

        int EndRead(IAsyncResult asyncResult);
    }
}