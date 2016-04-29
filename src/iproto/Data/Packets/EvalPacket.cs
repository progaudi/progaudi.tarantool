namespace iproto.Data.Packets
{
    public class EvalPacket<T> : UnifiedPacket
        where T : ITuple
    {
        public EvalPacket(string expression, T tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public T Tuple { get; }
    }
}