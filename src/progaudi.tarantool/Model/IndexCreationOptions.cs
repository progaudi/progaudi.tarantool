using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    [MsgPackMap]
    public class IndexCreationOptions
    {
        public IndexCreationOptions(bool unique)
        {
            Unique = unique;
        }

        public IndexCreationOptions()
        {
        }

        [MsgPackMapElement("unique")]
        public bool Unique { get; set; }
    }
}