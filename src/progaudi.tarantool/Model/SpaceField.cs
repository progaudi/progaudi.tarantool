using MessagePack;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackObject]
    public class SpaceField
    {
        public SpaceField(string name, FieldType type)
        {
            Name = name;
            Type = type;
        }

        [Key("name")]
        public string Name { get; }

        [Key("type")]
        public FieldType Type { get; }

        public override string ToString()
        {
            return $"[{Name}, {Type}]";
        }
    }
}