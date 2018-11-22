namespace ProGaudi.Tarantool.Client.Model
{
    public readonly struct RequestId
    {
        public RequestId(ulong value) => Value = value;

        public ulong Value { get; }

        public static implicit operator ulong(RequestId id) => id.Value;

        public static explicit operator RequestId(ulong id) => new RequestId(id);

        public override string ToString() => Value.ToString();
    }
}