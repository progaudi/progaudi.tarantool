using System;

namespace ProGaudi.Tarantool.Client
{
    internal interface IRequestWriter : IDisposable
    {
        void BeginWriting();

        bool IsConnected { get; }

        void Write(byte[] header, byte[] body);
    }
}