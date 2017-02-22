using ProGaudi.MsgPack.Light;
using System;
using System.Reflection;
using System.Text;

namespace ProGaudi.Tarantool.Client.Model
{
    /// <summary>Represents values that can be stored in tarantool</summary>
    public struct TarantoolValue
    {
        private static readonly byte[] EmptyByteArr = new byte[0];
        private readonly byte[] valueBlob;

        /// <summary>
        /// Represents the string <c>""</c>
        /// </summary>
        public static TarantoolValue EmptyString { get; } = new TarantoolValue(TarantoolValue.EmptyByteArr);

        /// <summary>A null value</summary>
        public static TarantoolValue Null { get; } = new TarantoolValue((byte[])null);
        
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
                return this.valueBlob == null || this.valueBlob.Length == 0;
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
        
        public TarantoolValue(byte[] valueBlob) : this()
        {
            this.valueBlob = valueBlob;
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
            return value.valueBlob;
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
        
        private static bool TryParseDouble(byte[] blob, out double value)
        {
            if (blob.Length != 1 || (int)blob[0] < 48 || (int)blob[0] > 57)
                return Format.TryParseDouble(Encoding.UTF8.GetString(blob), out value);
            value = (double)((int)blob[0] - 48);
            return true;
        }
        
        public T Unpack<T>(MsgPackContext context)
        {
            return MsgPackSerializer.Deserialize<T>(this.valueBlob, context);
        }
    }
}
