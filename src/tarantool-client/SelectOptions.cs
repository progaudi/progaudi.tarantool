using tarantool_client.IProto.Data;

namespace tarantool_client
{
    public class SelectOptions
    {
        public Iterator Iterator { get; set; } = Iterator.Eq;

        public uint Limit { get; set; } = uint.MaxValue;

        public uint Offset { get; set; } = 0;
    }
}