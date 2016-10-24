using System;

namespace ProGaudi.Tarantool.Client
{
    public interface IResponseReader : IDisposable
    {
        void BeginReading();
    }
}