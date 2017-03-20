using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model.Enums
{
    public enum FieldType
    {
        Str,
        Num,
        [StringValue("*")]
        Any
    }
}