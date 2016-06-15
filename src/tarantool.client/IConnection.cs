using System;
using System.Net;
using System.Threading.Tasks;

namespace Tarantool.Client
{
    internal interface IConnection : IDisposable
    {
        Task ConnectAsync(EndPoint endPoint);

        Task<byte[]> SendAsync(byte[] request, ulong requestId);
    }
}