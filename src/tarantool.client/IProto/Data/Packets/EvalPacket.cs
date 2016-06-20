namespace Tarantool.Client.IProto.Data.Packets
{
    public class EvalPacket<T> : IRequestPacket
        where T : ITuple
    {
        public EvalPacket(string expression, T tuple)
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public T Tuple { get; }

        public CommandCode Code => CommandCode.Eval;
    }
}