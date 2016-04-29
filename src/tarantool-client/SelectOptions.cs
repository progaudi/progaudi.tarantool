using iproto.Data;

namespace tarantool_client
{
    public class SelectOptions
    {
        public Iterator Iterator { get; set; } = Iterator.Eq;

        public uint Limit { get; set; } = 0;

        public uint Offset { get; set; } = 0;
    }
}