using System;
using Int8 = System.SByte;
using UInt8 = System.Byte;

namespace TarantoolDnx.MsgPack
{
    internal static class IntFormatTypeExtensions
    {
        private const sbyte Max5BitNegative = 1 - (1 << 6);

        public static IntFormatType GetFormatType(this long value)
        {
            if (value >= 0)
            {
                if (value <= Int8.MaxValue)
                    return IntFormatType.PositiveFixNum;

                if (value <= UInt8.MaxValue)
                    return IntFormatType.UInt8;

                if (value <= Int16.MaxValue)
                    return IntFormatType.Int16;

                if (value <= UInt16.MaxValue)
                    return IntFormatType.UInt16;

                if (value <= Int32.MaxValue)
                    return IntFormatType.Int32;

                if (value <= UInt32.MaxValue)
                    return IntFormatType.UInt32;

                return IntFormatType.Int64;
            }

            if (value >= Max5BitNegative)
            {
                return IntFormatType.NegativeFixNum;
            }

            if (value >= Int8.MinValue)
            {
                return IntFormatType.Int8;
            }

            if (value >= Int16.MinValue)
            {
                return IntFormatType.Int16;
            }

            if (value >= Int32.MinValue)
            {
                return IntFormatType.Int32;
            }

            return IntFormatType.Int64;
        }

        public static IntFormatType GetFormatType(this ulong value)
        {
            if (value > long.MaxValue)
                return IntFormatType.UInt64;

            return ((long) value).GetFormatType();
        }

        public static IntFormatType GetFormatType(this int value)
        {
            if (value >= 0)
            {
                if (value <= Int8.MaxValue)
                    return IntFormatType.PositiveFixNum;

                if (value <= UInt8.MaxValue)
                    return IntFormatType.UInt8;

                if (value <= Int16.MaxValue)
                    return IntFormatType.Int16;

                if (value <= UInt16.MaxValue)
                    return IntFormatType.UInt16;

                return IntFormatType.Int32;
            }

            if (value >= Max5BitNegative)
            {
                return IntFormatType.NegativeFixNum;
            }

            if (value >= Int8.MinValue)
            {
                return IntFormatType.Int8;
            }

            if (value >= Int16.MinValue)
            {
                return IntFormatType.Int16;
            }

            return IntFormatType.Int32;
        }

        public static IntFormatType GetFormatType(this uint value)
        {
            if (value > int.MaxValue)
                return IntFormatType.UInt32;

            return ((int)value).GetFormatType();
        }

        public static IntFormatType GetFormatType(this short value)
        {
            if (value >= 0)
            {
                if (value <= Int8.MaxValue)
                    return IntFormatType.PositiveFixNum;

                if (value <= UInt8.MaxValue)
                    return IntFormatType.UInt8;

                return IntFormatType.Int16;
            }

            if (value >= Max5BitNegative)
            {
                return IntFormatType.NegativeFixNum;
            }

            if (value >= Int8.MinValue)
            {
                return IntFormatType.Int8;
            }

            return IntFormatType.Int16;
        }

        public static IntFormatType GetFormatType(this ushort value)
        {
            if (value > short.MaxValue)
                return IntFormatType.UInt16;

            return ((short)value).GetFormatType();
        }

        public static IntFormatType GetFormatType(this sbyte value)
        {
            if (value >= 0)
            {
                return IntFormatType.PositiveFixNum;
            }

            if (value >= Max5BitNegative)
            {
                return IntFormatType.NegativeFixNum;
            }

            return IntFormatType.Int8;
        }

        public static IntFormatType GetFormatType(this byte value)
        {
            if (value > sbyte.MaxValue)
                return IntFormatType.UInt8;

            return ((short)value).GetFormatType();
        }
    }
}