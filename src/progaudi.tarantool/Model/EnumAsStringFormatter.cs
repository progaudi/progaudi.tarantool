using System;
using System.Collections.Generic;
using MessagePack;
using MessagePack.Formatters;

namespace ProGaudi.Tarantool.Client.Model
{
    public sealed class EnumAsStringFormatter<T> : IMessagePackFormatter<T>
    {
        private readonly bool _ignoreCase;
        private readonly Dictionary<string, T> _nameValueMapping;
        private readonly Dictionary<T, string> _valueNameMapping;

        public EnumAsStringFormatter(bool ignoreCase)
        {
            _ignoreCase = ignoreCase;
            var names = Enum.GetNames(typeof(T));
            var values = Enum.GetValues(typeof(T));

            _nameValueMapping = new Dictionary<string, T>(names.Length, ignoreCase ? (IEqualityComparer<string>) IgnoreCaseComparer.Instance : EqualityComparer<string>.Default);
            _valueNameMapping = new Dictionary<T, string>(names.Length);

            for (var i = 0; i < names.Length; i++)
            {
                _nameValueMapping[names[i]] = (T)values.GetValue(i);
                _valueNameMapping[(T)values.GetValue(i)] = names[i];
            }
        }

        public int Serialize(ref byte[] bytes, int offset, T value, IFormatterResolver formatterResolver)
        {
            if (!_valueNameMapping.TryGetValue(value, out var name))
            {
                name = value.ToString(); // fallback for flags etc, But Enum.ToString is too slow.
            }

            return MessagePackBinary.WriteString(ref bytes, offset, name);
        }

        public T Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
        {
            var name = MessagePackBinary.ReadString(bytes, offset, out readSize);

            if (!_nameValueMapping.TryGetValue(name, out var value))
            {
                value = (T)Enum.Parse(typeof(T), name, _ignoreCase); // Enum.Parse is too slow
            }

            return value;
        }

        private class IgnoreCaseComparer : IEqualityComparer<string>
        {
            public static readonly IgnoreCaseComparer Instance = new IgnoreCaseComparer();

            private IgnoreCaseComparer()
            {
            }

            public bool Equals(string x, string y)
            {
                return string.Compare(x, y, StringComparison.InvariantCultureIgnoreCase) == 0;
            }

            public int GetHashCode(string obj)
            {
                return obj.ToLowerInvariant().GetHashCode();
            }
        }
    }
}