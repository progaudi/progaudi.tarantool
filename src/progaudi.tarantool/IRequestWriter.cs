using System;
using ProGaudi.Tarantool.Client.Model.Requests;

namespace ProGaudi.Tarantool.Client
{
    public interface IRequestWriter : IDisposable
    {
        void Start();

        void Write(Request request);
    }
}