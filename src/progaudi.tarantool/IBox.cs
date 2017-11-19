using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Responses;

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

        Task Call_1_6(string functionName);

        Task Call_1_6<TTuple>(string functionName, TTuple parameters);

        Task<DataResponse<TResponse[]>> Call_1_6<TResponse>(string functionName);

        Task<DataResponse<TResponse[]>> Call_1_6<TTuple, TResponse>(string functionName, TTuple parameters);

        Task Call(string functionName);

        Task Call<TTuple>(string functionName, TTuple parameters);

        Task<DataResponse<TResponse[]>> Call<TResponse>(string functionName);

        Task<DataResponse<TResponse[]>> Call<TTuple, TResponse>(string functionName, TTuple parameters);

        Task<DataResponse<TResponse[]>> Eval<TTuple, TResponse>(string expression, TTuple parameters);

        Task<DataResponse<TResponse[]>> Eval<TResponse>(string expression);

        Task<byte[]> ExecuteSql(string query, params SqlParameter[] parameters);

        Task<DataResponse<TResponse[]>> ExecuteSql<TResponse>(string query, params SqlParameter[] parameters);
    }
}