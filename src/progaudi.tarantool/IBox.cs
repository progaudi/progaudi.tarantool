using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    public interface IBox : IDisposable
    {
        Task Connect();

        bool IsConnected { get; }

        Metrics Metrics { get; }

        ISchema Schema { get; }

        BoxInfo Info { get; }

        [Obsolete]
        ISchema GetSchema();

        Task ReloadSchema();

        Task ReloadBoxInfo();
    }
}