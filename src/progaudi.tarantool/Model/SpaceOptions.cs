using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    [MsgPackMap]
    public class SpaceOptions
    {
        [MsgPackMapElement("temporary")]
        public bool Temporary { get; set; }
    }
}