using System;

namespace Tarantool.Client
{
    public interface IResponseReader : IDisposable
    {
        void BeginReading();
    }
}