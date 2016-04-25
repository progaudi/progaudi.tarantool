using System;

namespace iproto.Data.Packets
{
    public class EvalPacket<T1> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1> Tuple { get; }
    }

    public class EvalPacket<T1, T2> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2> Tuple { get; }
    }

    public class EvalPacket<T1, T2, T3> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2, T3> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2, T3> Tuple { get; }
    }

    public class EvalPacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2, T3, T4> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2, T3, T4> Tuple { get; }
    }

    public class EvalPacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2, T3, T4, T5> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2, T3, T4, T5> Tuple { get; }
    }

    public class EvalPacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2, T3, T4, T5, T6> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Tuple { get; }
    }

    public class EvalPacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple { get; }
    }

    public class EvalPacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public EvalPacket(string expression, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            : base(new Header(CommandCode.Eval, null, null))
        {
            Expression = expression;
            Tuple = tuple;
        }

        public string Expression { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Tuple { get; }
    }
}