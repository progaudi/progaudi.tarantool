using System;
using JetBrains.Annotations;

namespace ProGaudi.Tarantool.Client.Model.Responses
{
    public class FieldMetadata : IEquatable<FieldMetadata>
    {
        public string Name { get; }

        public FieldMetadata([NotNull] string name)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
        }

        public override string ToString() => Name;

        public bool Equals(FieldMetadata other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((FieldMetadata) obj);
        }

        public override int GetHashCode()
        {
            return Name.GetHashCode();
        }

        public static bool operator ==(FieldMetadata left, FieldMetadata right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(FieldMetadata left, FieldMetadata right)
        {
            return !Equals(left, right);
        }
    }
}