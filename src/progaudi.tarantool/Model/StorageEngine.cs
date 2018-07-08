namespace ProGaudi.Tarantool.Client.Model
{
    [EnumAsString(true)]
    public enum StorageEngine
    {
        Memtx,
        Vinyl,
        Sysview
    }
}