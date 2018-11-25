using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client
{
    public interface IPhysicalConnection : IDisposable
    {
        Task<ReadOnlyMemory<byte>> Connect(ClientOptions options);

        bool IsConnected { get; }
        
        IResponseReader Reader { get; }
        
        IRequestWriter Writer { get; }
        
        ITaskSource TaskSource { get; }
    }
}