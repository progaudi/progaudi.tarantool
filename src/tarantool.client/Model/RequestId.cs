namespace Tarantool.Client.Model
{
    public struct RequestId
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
    }
}