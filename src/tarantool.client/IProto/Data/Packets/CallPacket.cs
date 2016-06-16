namespace Tarantool.Client.IProto.Data.Packets
{
    public class CallPacket<T> : IRequestPacket
        where T : ITuple
    {
        public CallPacket(string functionName, T tuple)
        {
            FunctionName = functionName;
            Tuple = tuple;
        }

        public string FunctionName { get; }

        public T Tuple { get; }

        public CommandCode Code => CommandCode.Call;
    }
}