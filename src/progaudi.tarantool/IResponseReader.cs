using System;
using System.IO;
using System.Threading.Tasks;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IResponseReader : IDisposable
    {
        void BeginReading();

        Task<(byte[] result, int bodyStart)> GetResponseTask(RequestId requestId);

        bool IsConnected { get; }
    }
}