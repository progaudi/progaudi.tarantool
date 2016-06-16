using System;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    public interface IPhysicalConnection : IDisposable
    {
        void Connect(ConnectionOptions options);

        Task<int> ReadAsync(byte[] buffer, int offset, int count);

        int Read(byte[] buffer, int offset, int count);

        Task WriteAsync(byte[] buffer, int offset, int count);

        void Write(byte[] buffer, int offset, int count);
    }
}