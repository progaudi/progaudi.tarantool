namespace Tarantool.Client.Model
{
    internal class PacketSize
    {
        public PacketSize(uint value)
        {
            Value = value;
        }

        public uint Value { get; } 
    }
}