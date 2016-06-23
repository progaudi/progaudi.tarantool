using Tarantool.Client.Utils;

namespace Tarantool.Client.Model.Enums
{
    public enum FieldType
    {
        Str,
        Num,
        [StringValue("*")]
        Any
    }
}