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

        ILuaCode<TResult> GetLuaFunc<TResult>(string name);

        ILuaCode<TResult> GetLuaCode<TResult>(string code);

        BoxInfo Info { get; }

        Task ReloadBoxInfo();
    }
}