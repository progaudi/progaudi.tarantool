using System;

namespace ProGaudi.Tarantool.Client.Model
{
    public readonly struct RequestId : IEquatable<RequestId>
    {
        public RequestId(ulong value)
        {
            Value = value;
        }

        public ulong Value { get; }

        public static implicit operator ulong(RequestId id)
        {
            return id.Value;
        }

        public static explicit operator RequestId(ulong id)
        {
            return new RequestId(id);
        }

        public override string ToString()
        {
            return Value.ToString();
        }

        public bool Equals(RequestId other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is RequestId id && Equals(id);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }
    }
}