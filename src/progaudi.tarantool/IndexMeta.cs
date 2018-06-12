using System.Collections.Generic;
using MessagePack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    // TODO: fix serializer to support struct.
    [MessagePackObject]
    public struct IndexMeta
    {
        [Key(0)]
        public uint SpaceId { get; set; }

        [Key(1)]
        public uint Id { get; set; }

        [Key(2)]
        public string Name { get; set; }

        [Key(3)]
        public IndexType Type { get; set; }

        [Key(4)]
        public IndexCreationOptions Options { get; set; }

        [Key(5)]
        public IndexPart[] Parts { get; set; }

        public override string ToString()
        {
            return $"Index: {Name} ({Id}), Unique: {Options.Unique}, Space: {SpaceId}, Parts: {Parts.Length}";
        }
    }
}