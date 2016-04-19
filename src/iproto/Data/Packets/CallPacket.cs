using System;

namespace iproto.Data.Packets
{
    public class CallPacket<T1> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1> Tuple { get; }
    }

    public class CallPacket<T1, T2> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2> Tuple { get; }
    }

    public class CallPacket<T1, T2, T3> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2, T3> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2, T3> Tuple { get; }
    }

    public class CallPacket<T1, T2, T3, T4> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2, T3, T4> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2, T3, T4> Tuple { get; }
    }

    public class CallPacket<T1, T2, T3, T4, T5> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2, T3, T4, T5> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2, T3, T4, T5> Tuple { get; }
    }

    public class CallPacket<T1, T2, T3, T4, T5, T6> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2, T3, T4, T5, T6> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2, T3, T4, T5, T6> Tuple { get; }
    }

    public class CallPacket<T1, T2, T3, T4, T5, T6, T7> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7> Tuple { get; }
    }

    public class CallPacket<T1, T2, T3, T4, T5, T6, T7, TRest> : UnifiedPacket
    {
        public CallPacket(Header header, string functionName, Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> tuple)
            : base(header)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public Tuple<T1, T2, T3, T4, T5, T6, T7, TRest> Tuple { get; }
    }
}