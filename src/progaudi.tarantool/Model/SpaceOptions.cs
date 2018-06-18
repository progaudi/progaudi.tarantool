using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    [MsgPackMap]
    public class SpaceOptions
    {
        public SpaceOptions()
        {
        }

        [MsgPackMapElement("temporary")]
        public bool Temporary { get; set; }

        public override string ToString()
        {
            return $"{nameof(Temporary)}: {Temporary}";
        }
    }
}