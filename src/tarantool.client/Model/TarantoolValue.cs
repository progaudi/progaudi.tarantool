using System;
using System.Reflection;
using System.Text;

namespace ProGaudi.Tarantool.Client.Model
{
    /// <summary>Represents values that can be stored in tarantool</summary>
    public struct TarantoolValue : IEquatable<TarantoolValue>, IComparable<TarantoolValue>, IComparable
    {
        internal static readonly TarantoolValue[] EmptyArray = new TarantoolValue[0];
        private static readonly byte[] EmptyByteArr = new byte[0];
        private static readonly byte[] IntegerSentinel = new byte[0];
        private readonly byte[] valueBlob;
        private readonly long valueInt64;

        /// <summary>
        /// Represents the string <c>""</c>
        /// </summary>
        public static TarantoolValue EmptyString { get; } = new TarantoolValue(0L, TarantoolValue.EmptyByteArr);

        /// <summary>A null value</summary>
        public static TarantoolValue Null { get; } = new TarantoolValue(0L, (byte[])null);

        /// <summary>Indicates whether the value is a primitive integer</summary>
        public bool IsInteger
        {
            get
            {
                return this.valueBlob == TarantoolValue.IntegerSentinel;
            }
        }

        /// <summary>
        /// Indicates whether the value should be considered a null value
        /// </summary>
        public bool IsNull
        {
            get
            {
                return this.valueBlob == null;
            }
        }

        /// <summary>
        /// Indicates whether the value is either null or a zero-length value
        /// </summary>
        public bool IsNullOrEmpty
        {
            get
            {
                if (this.valueBlob == null)
                    return true;
                if (this.valueBlob.Length == 0)
                    return this.valueBlob != TarantoolValue.IntegerSentinel;
                return false;
            }
        }

        /// <summary>
        /// Indicates whether the value is greater than zero-length
        /// </summary>
        public bool HasValue
        {
            get
            {
                if (this.valueBlob != null)
                    return (uint)this.valueBlob.Length > 0U;
                return false;
            }
        }

        private TarantoolValue(long valueInt64, byte[] valueBlob)
        {
            this.valueInt64 = valueInt64;
            this.valueBlob = valueBlob;
        }

        /// <summary>Creates a new tarantoolValue from an Int32</summary>
        public static implicit operator TarantoolValue(int value)
        {
            return new TarantoolValue((long)value, TarantoolValue.IntegerSentinel);
        }

        /// <summary>Creates a new tarantoolValue from a nullable Int32</summary>
        public static implicit operator TarantoolValue(int? value)
        {
            if (value.HasValue)
                return (TarantoolValue)value.GetValueOrDefault();
            return TarantoolValue.Null;
        }

        /// <summary>Creates a new tarantoolValue from an Int64</summary>
        public static implicit operator TarantoolValue(long value)
        {
            return new TarantoolValue(value, TarantoolValue.IntegerSentinel);
        }

        /// <summary>Creates a new tarantoolValue from a nullable Int64</summary>
        public static implicit operator TarantoolValue(long? value)
        {
            if (value.HasValue)
                return (TarantoolValue)value.GetValueOrDefault();
            return TarantoolValue.Null;
        }

        /// <summary>Creates a new tarantoolValue from a Double</summary>
        public static implicit operator TarantoolValue(double value)
        {
            return (TarantoolValue)Format.ToString(value);
        }

        /// <summary>Creates a new tarantoolValue from a nullable Double</summary>
        public static implicit operator TarantoolValue(double? value)
        {
            if (value.HasValue)
                return (TarantoolValue)value.GetValueOrDefault();
            return TarantoolValue.Null;
        }

        /// <summary>Creates a new tarantoolValue from a String</summary>
        public static implicit operator TarantoolValue(string value)
        {
            return new TarantoolValue(0L, value != null ? (value.Length != 0 ? Encoding.UTF8.GetBytes(value) : TarantoolValue.EmptyByteArr) : (byte[])null);
        }

        /// <summary>Creates a new tarantoolValue from a Byte[]</summary>
        public static implicit operator TarantoolValue(byte[] value)
        {
            return new TarantoolValue(0L, value != null ? (value.Length != 0 ? value : TarantoolValue.EmptyByteArr) : (byte[])null);
        }

        /// <summary>Creates a new tarantoolValue from a Boolean</summary>
        public static implicit operator TarantoolValue(bool value)
        {
            return new TarantoolValue(value ? 1L : 0L, TarantoolValue.IntegerSentinel);
        }

        /// <summary>Creates a new tarantoolValue from a nullable Boolean</summary>
        public static implicit operator TarantoolValue(bool? value)
        {
            if (value.HasValue)
                return (TarantoolValue)value.GetValueOrDefault();
            return TarantoolValue.Null;
        }

        /// <summary>Converts the value to a Boolean</summary>
        public static explicit operator bool(TarantoolValue value)
        {
            switch ((long)value)
            {
                case 0:
                    return false;
                case 1:
                    return true;
                default:
                    throw new InvalidCastException();
            }
        }

        /// <summary>Converts the value to an Int32</summary>
        public static explicit operator int(TarantoolValue value)
        {
            return checked((int)(long)value);
        }

        /// <summary>Converts the value to an Int64</summary>
        public static explicit operator long(TarantoolValue value)
        {
            byte[] numArray = value.valueBlob;
            if (numArray == TarantoolValue.IntegerSentinel)
                return value.valueInt64;
            if (numArray == null)
                return 0;
            long result;
            if (TarantoolValue.TryParseInt64(numArray, 0, numArray.Length, out result))
                return result;
            throw new InvalidCastException();
        }

        /// <summary>Converts the value to a Double</summary>
        public static explicit operator double(TarantoolValue value)
        {
            byte[] blob = value.valueBlob;
            if (blob == TarantoolValue.IntegerSentinel)
                return (double)value.valueInt64;
            if (blob == null)
                return 0.0;
            double num;
            if (TarantoolValue.TryParseDouble(blob, out num))
                return num;
            throw new InvalidCastException();
        }

        /// <summary>Converts the value to a nullable Double</summary>
        public static explicit operator double? (TarantoolValue value)
        {
            if (value.valueBlob == null)
                return new double?();
            return new double?((double)value);
        }

        /// <summary>Converts the value to a nullable Int64</summary>
        public static explicit operator long? (TarantoolValue value)
        {
            if (value.valueBlob == null)
                return new long?();
            return new long?((long)value);
        }

        /// <summary>Converts the value to a nullable Int32</summary>
        public static explicit operator int? (TarantoolValue value)
        {
            if (value.valueBlob == null)
                return new int?();
            return new int?((int)value);
        }

        /// <summary>Converts the value to a nullable Boolean</summary>
        public static explicit operator bool? (TarantoolValue value)
        {
            if (value.valueBlob == null)
                return new bool?();
            return new bool?((bool)value);
        }

        /// <summary>Converts the value to a String</summary>
        public static implicit operator string(TarantoolValue value)
        {
            byte[] bytes = value.valueBlob;
            if (bytes == TarantoolValue.IntegerSentinel)
                return Format.ToString(value.valueInt64);
            if (bytes == null)
                return (string)null;
            if (bytes.Length == 0)
                return "";
            try
            {
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return BitConverter.ToString(bytes);
            }
        }

        /// <summary>Converts the value to a byte[]</summary>
        public static implicit operator byte[] (TarantoolValue value)
        {
            byte[] numArray = value.valueBlob;
            if (numArray == TarantoolValue.IntegerSentinel)
                return Encoding.UTF8.GetBytes(Format.ToString(value.valueInt64));
            return numArray;
        }

        /// <summary>
        /// Indicates whether two tarantoolValue values are equivalent
        /// </summary>
        public static bool operator !=(TarantoolValue x, TarantoolValue y)
        {
            return !(x == y);
        }

        /// <summary>
        /// Indicates whether two tarantoolValue values are equivalent
        /// </summary>
        public static bool operator ==(TarantoolValue x, TarantoolValue y)
        {
            if (x.valueBlob == null)
                return y.valueBlob == null;
            if (x.valueBlob == TarantoolValue.IntegerSentinel)
            {
                if (y.valueBlob == TarantoolValue.IntegerSentinel)
                    return x.valueInt64 == y.valueInt64;
                return TarantoolValue.Equals((byte[])x, (byte[])y);
            }
            if (y.valueBlob == TarantoolValue.IntegerSentinel)
                return TarantoolValue.Equals((byte[])x, (byte[])y);
            return TarantoolValue.Equals(x.valueBlob, y.valueBlob);
        }

        /// <summary>See Object.Equals()</summary>
        public override bool Equals(object obj)
        {
            if (obj == null)
                return this.valueBlob == null;
            if (obj is TarantoolValue)
                return this.Equals((TarantoolValue)obj);
            if (obj is string)
                return (string)obj == (string)this;
            if (obj is byte[])
                return TarantoolValue.Equals((byte[])obj, (byte[])this);
            if (obj is long)
                return (long)obj == (long)this;
            if (obj is int)
                return (int)obj == (int)this;
            return false;
        }

        /// <summary>
        /// Indicates whether two tarantoolValue values are equivalent
        /// </summary>
        public bool Equals(TarantoolValue other)
        {
            return this == other;
        }

        /// <summary>See Object.GetHashCode()</summary>
        public override int GetHashCode()
        {
            if (this.valueBlob == TarantoolValue.IntegerSentinel)
                return this.valueInt64.GetHashCode();
            if (this.valueBlob == null)
                return -1;
            return TarantoolValue.GetHashCode(this.valueBlob);
        }

        /// <summary>Returns a string representation of the value</summary>
        public override string ToString()
        {
            return (string)this;
        }

        internal static unsafe bool Equals(byte[] x, byte[] y)
        {
            if (x == y)
                return true;
            if (x == null || y == null)
                return false;
            int length = x.Length;
            if (length != y.Length)
                return false;
            int num1 = length / 8;
            int num2 = length % 8;
            fixed (byte* numPtr1 = x)
            fixed (byte* numPtr2 = y)
            {
                long* numPtr3 = (long*)numPtr1;
                long* numPtr4 = (long*)numPtr2;
                for (int index = 0; index < num1; ++index)
                {
                    if (numPtr3[index] != numPtr4[index])
                        return false;
                }
                int index1 = length - num2;
                while (num2-- != 0)
                {
                    if ((int)numPtr1[index1] != (int)numPtr2[index1++])
                        return false;
                }
            }
            return true;
        }

        internal static unsafe int GetHashCode(byte[] value)
        {
            if (value == null)
                return -1;
            int length = value.Length;
            if (length == 0)
                return 0;
            int num1 = length / 8;
            int num2 = length % 8;
            int num3 = 728271210;
            fixed (byte* numPtr = value)
            {
                for (int index = 0; index < num1; ++index)
                {
                    long num4 = ((long*)numPtr)[index];
                    int num5 = (int)num4 ^ (int)(num4 >> 32);
                    num3 = (num3 << 5) + num3 ^ num5;
                }
                int num6 = length - num2;
                while (num2-- != 0)
                    num3 = (num3 << 5) + num3 ^ (int)numPtr[num6++];
            }
            return num3;
        }

        internal static bool TryParseInt64(byte[] value, int offset, int count, out long result)
        {
            result = 0L;
            if (value == null || count <= 0)
                return false;
            int num1 = checked(offset + count);
            if ((int)value[offset] == 45)
            {
                int index = checked(offset + 1);
                while (index < num1)
                {
                    byte num2 = value[index];
                    if ((int)num2 < 48 || (int)num2 > 57)
                        return false;
                    result = checked(result * 10L - (long)((int)num2 - 48));
                    checked { ++index; }
                }
                return true;
            }
            int index1 = offset;
            while (index1 < num1)
            {
                byte num2 = value[index1];
                if ((int)num2 < 48 || (int)num2 > 57)
                    return false;
                result = checked(result * 10L + (long)((int)num2 - 48));
                checked { ++index1; }
            }
            return true;
        }

        internal void AssertNotNull()
        {
            if (this.IsNull)
                throw new ArgumentException("A null value is not valid in this context");
        }

        private TarantoolValue.CompareType ResolveType(out long i64, out double r8)
        {
            byte[] blob = this.valueBlob;
            if (blob == TarantoolValue.IntegerSentinel)
            {
                i64 = this.valueInt64;
                r8 = 0.0;
                return TarantoolValue.CompareType.Int64;
            }
            if (blob == null)
            {
                i64 = 0L;
                r8 = 0.0;
                return TarantoolValue.CompareType.Null;
            }
            if (TarantoolValue.TryParseInt64(blob, 0, blob.Length, out i64))
            {
                r8 = 0.0;
                return TarantoolValue.CompareType.Int64;
            }
            if (TarantoolValue.TryParseDouble(blob, out r8))
            {
                i64 = 0L;
                return TarantoolValue.CompareType.Double;
            }
            i64 = 0L;
            r8 = 0.0;
            return TarantoolValue.CompareType.Raw;
        }

        /// <summary>Compare against a tarantoolValue for relative order</summary>
        public int CompareTo(TarantoolValue other)
        {
            try
            {
                long i64_1;
                double r8_1;
                TarantoolValue.CompareType compareType1 = this.ResolveType(out i64_1, out r8_1);
                long i64_2;
                double r8_2;
                TarantoolValue.CompareType compareType2 = other.ResolveType(out i64_2, out r8_2);
                if (compareType1 == TarantoolValue.CompareType.Null)
                    return compareType2 == TarantoolValue.CompareType.Null ? 0 : -1;
                if (compareType2 == TarantoolValue.CompareType.Null)
                    return 1;
                if (compareType1 == TarantoolValue.CompareType.Int64)
                {
                    if (compareType2 == TarantoolValue.CompareType.Int64)
                        return i64_1.CompareTo(i64_2);
                    if (compareType2 == TarantoolValue.CompareType.Double)
                        return ((double)i64_1).CompareTo(r8_2);
                }
                else if (compareType1 == TarantoolValue.CompareType.Double)
                {
                    if (compareType2 == TarantoolValue.CompareType.Int64)
                        return r8_1.CompareTo((double)i64_2);
                    if (compareType2 == TarantoolValue.CompareType.Double)
                        return r8_1.CompareTo(r8_2);
                }
                return StringComparer.Ordinal.Compare((string)this, (string)other);
            }
            catch (Exception ex)
            {
            }
            return 0;
        }

        int IComparable.CompareTo(object obj)
        {
            if (obj is TarantoolValue)
                return this.CompareTo((TarantoolValue)obj);
            if (obj is long)
                return this.CompareTo((TarantoolValue)((long)obj));
            if (obj is double)
                return this.CompareTo((TarantoolValue)((double)obj));
            if (obj is string)
                return this.CompareTo((TarantoolValue)((string)obj));
            if (obj is byte[])
                return this.CompareTo((TarantoolValue)((byte[])obj));
            if (obj is bool)
                return this.CompareTo((TarantoolValue)((bool)obj));
            return -1;
        }

        private static bool TryParseDouble(byte[] blob, out double value)
        {
            if (blob.Length != 1 || (int)blob[0] < 48 || (int)blob[0] > 57)
                return Format.TryParseDouble(Encoding.UTF8.GetString(blob), out value);
            value = (double)((int)blob[0] - 48);
            return true;
        }
        
        /// <summary>
        /// Convert to a long if possible, returning true.
        /// 
        /// Returns false otherwise.
        /// </summary>
        public bool TryParse(out long val)
        {
            byte[] numArray = this.valueBlob;
            if (numArray == TarantoolValue.IntegerSentinel)
            {
                val = this.valueInt64;
                return true;
            }
            if (numArray != null)
                return TarantoolValue.TryParseInt64(numArray, 0, numArray.Length, out val);
            val = 0L;
            return true;
        }

        /// <summary>
        /// Convert to a int if possible, returning true.
        /// 
        /// Returns false otherwise.
        /// </summary>
        public bool TryParse(out int val)
        {
            long val1;
            if (!this.TryParse(out val1) || val1 > (long)int.MaxValue || val1 < (long)int.MinValue)
            {
                val = 0;
                return false;
            }
            val = (int)val1;
            return true;
        }

        /// <summary>
        /// Convert to a double if possible, returning true.
        /// 
        /// Returns false otherwise.
        /// </summary>
        public bool TryParse(out double val)
        {
            byte[] blob = this.valueBlob;
            if (blob == TarantoolValue.IntegerSentinel)
            {
                val = (double)this.valueInt64;
                return true;
            }
            if (blob != null)
                return TarantoolValue.TryParseDouble(blob, out val);
            val = 0.0;
            return true;
        }

        public T Unpack<T>()
        {
            throw new NotImplementedException();
        }

        public void Pack<T>(T value)
        {
            throw new NotImplementedException();
        }

        private enum CompareType
        {
            Null,
            Int64,
            Double,
            Raw,
        }
    }
}
