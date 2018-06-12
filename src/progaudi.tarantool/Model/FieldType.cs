namespace ProGaudi.Tarantool.Client.Model
{
    [EnumAsString(true)]
    public enum FieldType
    {
        Str,
        Num,
        Unsigned,
        String,
        Scalar,
        Map,
        Array
    }
}