using Tarantool.Client.Model.Enums;

namespace Tarantool.Client.Model.Requests
{
    public class EvalRequest<T> : IRequest
        where T : ITuple
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