using Tarantool.Client.Utils;

namespace Tarantool.Client
{
    public enum FieldType
    {
        Str,
        Num,
        [StringValue("*")]
        Any
    }
}