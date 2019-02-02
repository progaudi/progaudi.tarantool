using System;
using System.Buffers;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface ITaskSource : IDisposable
    {
        Task<ReadOnlySequence<byte>> GetResponseTask(in RequestId requestId);
        
        void MatchResult(in ReadOnlyMemory<byte> result);
        void MatchResult(in ReadOnlySequence<byte> sequence);
    }
}