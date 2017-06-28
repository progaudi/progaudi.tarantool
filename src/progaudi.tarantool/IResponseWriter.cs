using System;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client
{
    internal interface IResponseWriter : IDisposable
    {
        void BeginWriting();

        bool IsConnected { get; }

        Task Write(ArraySegment<byte> header, ArraySegment<byte> body);
    }
}