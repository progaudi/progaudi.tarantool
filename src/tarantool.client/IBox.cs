using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Responses;

namespace ProGaudi.Tarantool.Client
{
    public interface IBox
    {
        Task Connect();

        ISchema GetSchema();

        Task Call_1_6(string functionName);

        Task Call_1_6<TTuple>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TResponse[]>> Call_1_6<TResponse>(string functionName)
            where TResponse : ITarantoolTuple;

        Task<DataResponse<TResponse[]>> Call_1_6<TTuple, TResponse>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple
            where TResponse : ITarantoolTuple;

        Task Call(string functionName);

        Task Call<TTuple>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TResponse[]>> Call<TResponse>(string functionName);

        Task<DataResponse<TResponse[]>> Call<TTuple, TResponse>(string functionName, TTuple parameters)
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TResponse[]>> Eval<TTuple, TResponse>(string expression, TTuple parameters)
            where TTuple : ITarantoolTuple;

        Task<DataResponse<TResponse[]>> Eval<TResponse>(string expression);
    }
}