namespace iproto.Data.Packets
{
    public class CallPacket<T> : UnifiedPacket
        where T : ITuple
    {
        public CallPacket(string functionName, T tuple)
            : base(new Header(CommandCode.Call, null, null))
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public T Tuple { get; }
    }
}