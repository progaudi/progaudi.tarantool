using tarantool_client.Utils;

namespace tarantool_client
{
    public enum FieldType
    {
        Str,
        Num,
        [StringValue("*")]
        Any
    }
}