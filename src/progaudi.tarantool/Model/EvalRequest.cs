namespace ProGaudi.Tarantool.Client.Model
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

        public CommandCodes Code => CommandCodes.Eval;
    }
}