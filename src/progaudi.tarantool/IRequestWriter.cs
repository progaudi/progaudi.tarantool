using System;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client
{
    internal interface IRequestWriter : IDisposable
    {
        void BeginWriting();

        bool IsConnected { get; }

        void Write(ArraySegment<byte> request);
    }
}