using MessagePack;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackObject]
    public class SpaceOptions
    {
        [Key("temporary")]
        public bool Temporary { get; set; }

        public override string ToString()
        {
            return $"{nameof(Temporary)}: {Temporary}";
        }
    }
}