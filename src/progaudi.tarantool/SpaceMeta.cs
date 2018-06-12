using System;
using MessagePack;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    [MessagePackObject]
    public struct SpaceMeta : IEquatable<SpaceMeta>
    {
        public SpaceMeta(uint id, int ownerId, string name, StorageEngine engine, uint fieldCount, SpaceOptions options, SpaceField[] fields)
        {
            Id = id;
            OwnerId = ownerId;
            Name = name;
            Engine = engine;
            FieldCount = fieldCount;
            Options = options;
            Fields = fields;
        }

        [Key(0)]
        public uint Id { get; }

        [Key(1)]
        public int OwnerId { get; }

        [Key(2)]
        public string Name { get; }

        [Key(3)]
        public StorageEngine Engine { get; }

        [Key(4)]
        public uint FieldCount { get; }

        [Key(5)]
        public SpaceOptions Options { get; }

        [Key(6)]
        public SpaceField[] Fields { get; }

        public override string ToString()
        {
            return $"{Name}({Id}, {Engine}, {Options.Temporary}).";
        }

        public bool Equals(SpaceMeta other)
        {
            return Id == other.Id && OwnerId == other.OwnerId && string.Equals(Name, other.Name) && Engine == other.Engine && FieldCount == other.FieldCount;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is SpaceMeta && Equals((SpaceMeta) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (int) Id;
                hashCode = (hashCode * 397) ^ OwnerId;
                hashCode = (hashCode * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode * 397) ^ (int) Engine;
                hashCode = (hashCode * 397) ^ (int) FieldCount;
                return hashCode;
            }
        }
    }
}