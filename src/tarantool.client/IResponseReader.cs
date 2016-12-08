using System;

namespace ProGaudi.Tarantool.Client
{
    using System.IO;
    using System.Threading.Tasks;

    using ProGaudi.Tarantool.Client.Model;

    public interface IResponseReader : IDisposable
    {
        void BeginReading();

        Task<MemoryStream> GetResponseTask(RequestId requestId);

        bool IsConnected();
    }
}