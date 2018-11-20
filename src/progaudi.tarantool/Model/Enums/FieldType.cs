using ProGaudi.Tarantool.Client.Utils;

namespace ProGaudi.Tarantool.Client.Model.Enums
{
    public enum FieldType2
    {
        Str,
        Num,
        [StringValue("*")]
        Any,
        unsigned,
        array,
        
    }
}