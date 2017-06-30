using ProGaudi.Tarantool.Client.Model.Enums;

namespace ProGaudi.Tarantool.Client.Model.Requests
{
    public class EvalRequest<T> : IRequest
    {
        public EvalRequest(string expression, T tuple)
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public T Tuple { get; }

        public CommandCode Code => CommandCode.Eval;
    }
}