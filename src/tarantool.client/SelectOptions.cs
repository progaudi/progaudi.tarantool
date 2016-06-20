using Tarantool.Client.IProto.Data;

namespace Tarantool.Client
{
    public class SelectOptions
    {
        public Iterator Iterator { get; set; } = Iterator.Eq;

        public uint Limit { get; set; } = uint.MaxValue;

        public uint Offset { get; set; } = 0;
    }
}