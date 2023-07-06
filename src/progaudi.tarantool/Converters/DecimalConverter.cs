using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model.Enums;
using ProGaudi.Tarantool.Client.Utils;
using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace ProGaudi.Tarantool.Client.Converters
{
    /// <summary>
    /// Converter for Tarantool decimal values, implemented as MsgPack extension.
    /// Format described in https://www.tarantool.io/ru/doc/latest/dev_guide/internals/msgpack_extensions/#the-decimal-type
    /// Limitation: .NET decimal max scale is 28 digits, when Tarantool decimal max scale is 38 digits 
    /// </summary>
    public class DecimalConverter : IMsgPackConverter<decimal>
    {
        private static readonly byte[] SupportedFixTypes = new byte[5]
        {
            MsgPackExtDataTypes.FixExt1,
            MsgPackExtDataTypes.FixExt2,
            MsgPackExtDataTypes.FixExt4,
            MsgPackExtDataTypes.FixExt8,
            MsgPackExtDataTypes.FixExt16
        };
        private static readonly byte[] SupportedNonFixTypes = new byte[3]
        {
            MsgPackExtDataTypes.Ext8,
            MsgPackExtDataTypes.Ext16,
            MsgPackExtDataTypes.Ext32
        };

        private const byte MP_DECIMAL = 0x01;
        private const byte DECIMAL_PLUS = 0x0C;
        private const byte DECIMAL_MINUS = 0x0D;
        private const byte DECIMAL_MINUS_ALT = 0x0B;

        public void Initialize(MsgPackContext context)
        {
        }

        public decimal Read(IMsgPackReader reader)
        {
            var dataType = reader.ReadByte();
            var fixedDataType = true;
            var len = 0;
            switch (dataType)
            {
                case MsgPackExtDataTypes.Ext8:
                case MsgPackExtDataTypes.Ext16:
                case MsgPackExtDataTypes.Ext32:
                    fixedDataType = false;
                    break;
                case MsgPackExtDataTypes.FixExt1:
                    len = 1;
                    break;
                case MsgPackExtDataTypes.FixExt2:
                    len = 2;
                    break;
                case MsgPackExtDataTypes.FixExt4:
                    len = 4;
                    break;
                case MsgPackExtDataTypes.FixExt8:
                    len = 8;
                    break;
                case MsgPackExtDataTypes.FixExt16:
                    len = 16;
                    break;
                default:
                    throw ExceptionHelper.UnexpectedDataType(dataType, SupportedFixTypes.Union(SupportedNonFixTypes).ToArray());
            }

            if (!fixedDataType) 
            {
                len = reader.ReadByte();
            }

            var mpHeader = reader.ReadByte();
            if (mpHeader != MP_DECIMAL)
            {
                throw ExceptionHelper.UnexpectedMsgPackHeader(mpHeader, MP_DECIMAL);
            }

            var data = reader.ReadBytes((uint)len).ToArray();

            // used Java impl https://github.com/tarantool/cartridge-java/blob/1ca12332b870167b86d3e38891ab74527dfc8a19/src/main/java/io/tarantool/driver/mappers/converters/value/defaults/DefaultExtensionValueToBigDecimalConverter.java

            // Extract sign from the last nibble
            int signum = (byte)(SecondNibbleFromByte(data[len - 1]));
            if (signum == DECIMAL_MINUS || signum == DECIMAL_MINUS_ALT)
            {
                signum = -1;
            }
            else if (signum <= 0x09)
            {
                throw new IOException("The sign nibble has wrong value");
            }
            else
            {
                signum = 1;
            }

            int scale = data[0];
            if (scale > 28)
            {
                throw new OverflowException($"Maximum .NET decimal scale is exceeded. Maximum: 28. Actual: {scale}");
            }

            int skipIndex = 1; //skip byte with scale

            int digitsNum = (len - skipIndex) << 1;
            char digit = CharFromDigit(FirstNibbleFromByte(data[len - 1]), digitsNum - 1);

            char[] digits = new char[digitsNum];
            int pos = 2 * (len - skipIndex) - 1;

            digits[pos--] = digit;
            for (int i = len - 2; i >= skipIndex; i--)
            {
                digits[pos--] = CharFromDigit(SecondNibbleFromByte(data[i]), pos);
                digits[pos--] = CharFromDigit(FirstNibbleFromByte(data[i]), pos);
            }

            return CreateDecimalFromDigits(digits, scale, signum < 0);
        }

        public void Write(decimal value, IMsgPackWriter writer)
        {
            (int scale, decimal unscaledValue) = ExtractScaleFromDecimal(value);

            // used Java impl https://github.com/tarantool/cartridge-java/blob/1ca12332b870167b86d3e38891ab74527dfc8a19/src/main/java/io/tarantool/driver/mappers/converters/value/defaults/DefaultExtensionValueToBigDecimalConverter.java
            var unscaledValueStr = unscaledValue.ToString();
            byte signum = value >= 0 ? DECIMAL_PLUS : DECIMAL_MINUS;
            int digitsNum = unscaledValueStr.Length;
            
            int len = (digitsNum >> 1) + 1;
            byte[] payload = new byte[len];
            payload[len - 1] = signum;
            int pos = 0;
            char[] digits = unscaledValueStr.Substring(pos).ToCharArray();
            pos = digits.Length - 1;
            for (int i = len - 1; i > 0; i--)
            {
                payload[i] |= (byte)(DigitFromChar(digits[pos--]) << 4);
                payload[i - 1] |= (byte)DigitFromChar(digits[pos--]);
            }
            if (pos == 0)
            {
                payload[0] |= (byte)(DigitFromChar(digits[pos]) << 4);
            }

            writer.Write(MsgPackExtDataTypes.Ext8);
            writer.Write((byte)(len + 1));
            writer.Write(MP_DECIMAL);
            writer.Write((byte)scale);
            writer.Write(payload);
        }

        private static (int, decimal) ExtractScaleFromDecimal(decimal val)
        {
            var bits = decimal.GetBits(val);
            int scale = (bits[3] >> 16) & 0x7F;
            decimal unscaledValue = new Decimal(bits[0], bits[1], bits[2], false, 0);
            return (scale, unscaledValue);
        }

        private static int UnsignedRightShift(int signed, int places)
        {
            unchecked 
            {
                var unsigned = (uint)signed;
                unsigned >>= places;
                return (int)unsigned;
            }
        }

        private static int FirstNibbleFromByte(byte val)
        {
            return UnsignedRightShift(val & 0xF0, 4);
        }

        private static int SecondNibbleFromByte(byte val)
        {
            return val & 0x0F;
        }

        private static char CharFromDigit(int val, int pos)
        {
            var digit = (char)val;
            if (digit > 9)
            {
                throw new IOException(String.Format("Invalid digit at position %d", pos));
            }
            return digit;
        }

        private static int DigitFromChar(char val)
        {
            return val - '0';
        }

        private static decimal CreateDecimalFromDigits(char[] digits, int scale, bool isNegative)
        {
            int pos = 0;
            while (pos < digits.Length && digits[pos] == 0)
            {
                pos++;
            }

            if (pos == digits.Length) 
            {
                return 0;
            }

            StringBuilder sb = new StringBuilder();
            for (; pos < digits.Length; pos++)
            {
                sb.Append((int)digits[pos]);
            }

            if (scale >= sb.Length)
            {
                sb.Insert(0, String.Join("", Enumerable.Range(0, scale - sb.Length + 1).Select(_ => "0")));
            }

            if (scale > 0)
            {
                sb.Insert(sb.Length - scale, ".");
            }

            if (isNegative)
            {
                sb.Insert(0, '-');
            }
            return Decimal.Parse(sb.ToString(), CultureInfo.InvariantCulture);
        }
    }
}
