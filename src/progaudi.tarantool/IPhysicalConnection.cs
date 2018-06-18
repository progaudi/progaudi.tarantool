using System;
using System.IO;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IPhysicalConnection : IDisposable
    {
        Task Connect(ClientOptions options);

        Task Flush();

        bool IsConnected { get; }

        Task<int> ReadAsync(byte[] buffer, int offset, int count);

        void Write(in ArraySegment<byte> buffer);

        Stream Stream { get; }
    }
}