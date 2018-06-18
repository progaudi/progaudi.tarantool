using System;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    [MsgPackArray]
    public class IndexMeta : IEquatable<IndexMeta>
    {
        public IndexMeta(uint spaceId, uint id, string name, IndexType type, IndexCreationOptions options, IndexPart[] parts)
        {
            SpaceId = spaceId;
            Id = id;
            Name = name;
            Type = type;
            Options = options;
            Parts = parts;
        }

        public IndexMeta()
        {
        }

        [MsgPackArrayElement(0)]
        public uint SpaceId { get; set; }

        [MsgPackArrayElement(1)]
        public uint Id { get; set; }

        [MsgPackArrayElement(2)]
        public string Name { get; set; }

        [MsgPackArrayElement(3)]
        public IndexType Type { get; set; }

        [MsgPackArrayElement(4)]
        public IndexCreationOptions Options { get; set; }

        [MsgPackArrayElement(5)]
        public IndexPart[] Parts { get; set; }

        public override string ToString()
        {
            return $"Index: {Name} ({Id}), Unique: {Options.Unique}, Space: {SpaceId}, Parts: {Parts.Length}";
        }

        public bool Equals(IndexMeta other)
        {
            return SpaceId == other.SpaceId && Id == other.Id && string.Equals(Name, other.Name) && Type == other.Type;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IndexMeta meta && Equals(meta);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) SpaceId;
                hashCode = (hashCode * 397) ^ (int) Id;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Type;
                return hashCode;
            }
        }
    }
}