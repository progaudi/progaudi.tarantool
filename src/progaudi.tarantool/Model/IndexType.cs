namespace ProGaudi.Tarantool.Client.Model
{
    [EnumAsString(true)]
    public enum IndexType
    {
        Tree,
        Hash,
        Bitset,
        RTree
    }
}