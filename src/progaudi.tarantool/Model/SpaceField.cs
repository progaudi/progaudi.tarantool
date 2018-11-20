namespace ProGaudi.Tarantool.Client.Model
{
    public class SpaceField
    {
        public SpaceField(string name, string type)
        {
            Name = name;
            Type = type;
        }

        public string Name { get; }

        public string Type { get; }
    }
}