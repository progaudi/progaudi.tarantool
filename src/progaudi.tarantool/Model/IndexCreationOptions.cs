using MessagePack;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackObject]
    public class IndexCreationOptions
    {
        public IndexCreationOptions(bool unique)
        {
            Unique = unique;
        }

        [Key("unique")]
        public bool Unique { get; }
    }
}