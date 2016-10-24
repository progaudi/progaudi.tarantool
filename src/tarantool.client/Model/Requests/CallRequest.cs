using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class CallRequest<T> : IRequest
        where T : ITarantoolTuple
    {
        public CallRequest(string functionName, T tuple)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public T Tuple { get; }

        public CommandCode Code => CommandCode.Call;
    }
}