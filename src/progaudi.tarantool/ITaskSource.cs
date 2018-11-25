using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface ITaskSource : IDisposable
    {
        Task<ReadOnlyMemory<byte>> GetResponseTask(RequestId requestId);
        
        void MatchResult(ReadOnlyMemory<byte> result);
    }
}