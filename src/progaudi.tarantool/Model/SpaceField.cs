using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    [MsgPackMap]
    public class SpaceField
    {
        public SpaceField(string name, FieldType type)
        {
            Name = name;
            Type = type;
        }

        public SpaceField()
        {
        }

        [MsgPackMapElement("name")]
        public string Name { get; set; }

        [MsgPackMapElement("type")]
        public FieldType Type { get; set; }

        public override string ToString()
        {
            return $"[{Name}, {Type}]";
        }
    }
}