namespace Tarantool.Client.Model
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