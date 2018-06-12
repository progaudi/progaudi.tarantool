using MessagePack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    [MessagePackObject]
    public struct SpaceMeta
    {
        [Key(0)]
        public uint Id { get; set; }

        [Key(1)]
        public int OwnerId { get; set; }

        [Key(2)]
        public string Name { get; set; }

        [Key(3)]
        public StorageEngine Engine { get; set; }

        [Key(4)]
        public uint FieldCount { get; set; }

        [Key(5)]
        public SpaceOptions Options { get; set; }

        [Key(6)]
        public SpaceField[] Fields { get; set; }

        public override string ToString()
        {
            return $"{Name}({Id}, {Engine}, {Options.Temporary}).";
        }
    }
}