using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client
{
    public interface ILuaCode<TResult>
    {
        Task<TResult> Invoke<T>(T param);
        Task<TResult> Invoke<T1, T2>(T1 param1, T2 param2);
        Task<TResult> Invoke<T1, T2, T3>(T1 param1, T2 param2, T3 param3);
        Task<TResult> Invoke<T1, T2, T3, T4>(T1 param1, T2 param2, T3 param3, T4 param4);
        Task<TResult> Invoke<T1, T2, T3, T4, T5>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5);
        Task<TResult> Invoke<T1, T2, T3, T4, T5, T6>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6);
        Task<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7);
        Task<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8);
        Task<TResult> Invoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(T1 param1, T2 param2, T3 param3, T4 param4, T5 param5, T6 param6, T7 param7, T8 param8, T9 param9);
        
        Task<TResult> Invoke(params object[] parameters);
    }
}