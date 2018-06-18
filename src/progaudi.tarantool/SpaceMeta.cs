using System;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client
{
    [MsgPackArray]
    public class SpaceMeta : IEquatable<SpaceMeta>
    {
        public SpaceMeta()
        {
        }

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

        [MsgPackArrayElement(0)]
        public uint Id { get; set; }

        [MsgPackArrayElement(1)]
        public int OwnerId { get; set; }

        [MsgPackArrayElement(2)]
        public string Name { get; set; }

        [MsgPackArrayElement(3)]
        public StorageEngine Engine { get; set; }

        [MsgPackArrayElement(4)]
        public uint FieldCount { get; set; }

        [MsgPackArrayElement(5)]
        public SpaceOptions Options { get; set; }

        [MsgPackArrayElement(6)]
        public SpaceField[] Fields { get; set; }

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