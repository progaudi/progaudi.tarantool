using System;
using JetBrains.Annotations;
using MessagePack;
using MessagePack.Formatters;

namespace ProGaudi.Tarantool.Client.Model
{
    [MessagePackFormatter(typeof(Formatter))]
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

        public sealed class Formatter : IMessagePackFormatter<FieldMetadata>
        {
            public int Serialize(ref byte[] bytes, int offset, FieldMetadata value, IFormatterResolver formatterResolver)
            {
                throw new NotImplementedException();
            }

            public FieldMetadata Deserialize(byte[] bytes, int offset, IFormatterResolver formatterResolver, out int readSize)
            {
                if (MessagePackBinary.IsNil(bytes, offset))
                {
                    readSize = 1;
                    return null;
                }

                var startOffset = offset;
                var length = MessagePackBinary.ReadMapHeaderRaw(bytes, offset, out readSize);
                offset += readSize;

                var result = default(FieldMetadata);
                for (var i = 0; i < length; i++)
                {
                    var key = MessagePackBinary.ReadUInt32(bytes, offset, out readSize);
                    offset += readSize;
                    switch (key)
                    {
                        case Keys.FieldName:
                            result = new FieldMetadata(MessagePackBinary.ReadString(bytes, offset, out readSize));
                            offset += readSize;
                            break;
                        default:
                            offset += MessagePackBinary.ReadNext(bytes, offset);
                            break;
                    }
                }

                readSize = offset - startOffset;

                return result;
            }
        }
    }
}