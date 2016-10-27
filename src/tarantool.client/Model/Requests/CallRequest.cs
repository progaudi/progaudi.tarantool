using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class CallRequest<T> : IRequest
        where T : ITarantoolTuple
    {
        private readonly bool _use17;

        public CallRequest(string functionName, T tuple, bool use17 = true)
        {
            _use17 = use17;
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public T Tuple { get; }

        public CommandCode Code => _use17 ? CommandCode.Call : CommandCode.OldCall;
    }
}