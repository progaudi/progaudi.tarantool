namespace Tarantool.Client.IProto
{
    public class PacketSize
    {
        public PacketSize(uint value)
        {
            Value = value;
        }

        public uint Value { get; } 
    }
}