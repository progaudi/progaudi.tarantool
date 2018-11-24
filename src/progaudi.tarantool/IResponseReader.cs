using System;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IResponseReader : IDisposable
    {
        void Start();

        Task<ReadOnlyMemory<byte>> GetResponseTask(RequestId requestId);
    }
}