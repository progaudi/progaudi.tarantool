using System;
using Int8 = System.SByte;
using UInt8 = System.Byte;

// ReSharper disable BuiltInTypeReferenceStyle

namespace TarantoolDnx.MsgPack
{
    internal static class DataTypesExtensions
    {
        private const sbyte Max5BitNegative = 1 - (1 << 6);

        public static DataTypes GetFormatType(this long value)
        {
            if (value >= 0)
            {
                if (value <= Int8.MaxValue)
                    return DataTypes.PositiveFixNum;

                if (value <= UInt8.MaxValue)
                    return DataTypes.UInt8;

                if (value <= Int16.MaxValue)
                    return DataTypes.Int16;

                if (value <= UInt16.MaxValue)
                    return DataTypes.UInt16;

                if (value <= Int32.MaxValue)
                    return DataTypes.Int32;

                if (value <= UInt32.MaxValue)
                    return DataTypes.UInt32;

                return DataTypes.Int64;
            }

            if (value >= Max5BitNegative)
            {
                return DataTypes.NegativeFixNum;
            }

            if (value >= Int8.MinValue)
            {
                return DataTypes.Int8;
            }

            if (value >= Int16.MinValue)
            {
                return DataTypes.Int16;
            }

            if (value >= Int32.MinValue)
            {
                return DataTypes.Int32;
            }

            return DataTypes.Int64;
        }

        public static DataTypes GetFormatType(this ulong value)
        {
            if (value > long.MaxValue)
                return DataTypes.UInt64;

            return ((long)value).GetFormatType();
        }

        public static DataTypes GetFormatType(this int value)
        {
            if (value >= 0)
            {
                if (value <= Int8.MaxValue)
                    return DataTypes.PositiveFixNum;

                if (value <= UInt8.MaxValue)
                    return DataTypes.UInt8;

                if (value <= Int16.MaxValue)
                    return DataTypes.Int16;

                if (value <= UInt16.MaxValue)
                    return DataTypes.UInt16;

                return DataTypes.Int32;
            }

            if (value >= Max5BitNegative)
            {
                return DataTypes.NegativeFixNum;
            }

            if (value >= Int8.MinValue)
            {
                return DataTypes.Int8;
            }

            if (value >= Int16.MinValue)
            {
                return DataTypes.Int16;
            }

            return DataTypes.Int32;
        }

        public static DataTypes GetFormatType(this uint value)
        {
            if (value > int.MaxValue)
                return DataTypes.UInt32;

            return ((int)value).GetFormatType();
        }

        public static DataTypes GetFormatType(this short value)
        {
            if (value >= 0)
            {
                if (value <= Int8.MaxValue)
                    return DataTypes.PositiveFixNum;

                if (value <= UInt8.MaxValue)
                    return DataTypes.UInt8;

                return DataTypes.Int16;
            }

            if (value >= Max5BitNegative)
            {
                return DataTypes.NegativeFixNum;
            }

            if (value >= Int8.MinValue)
            {
                return DataTypes.Int8;
            }

            return DataTypes.Int16;
        }

        public static DataTypes GetFormatType(this ushort value)
        {
            if (value > short.MaxValue)
                return DataTypes.UInt16;

            return ((short)value).GetFormatType();
        }

        public static DataTypes GetFormatType(this sbyte value)
        {
            if (value >= 0)
            {
                return DataTypes.PositiveFixNum;
            }

            if (value >= Max5BitNegative)
            {
                return DataTypes.NegativeFixNum;
            }

            return DataTypes.Int8;
        }

        public static DataTypes GetFormatType(this byte value)
        {
            if (value > sbyte.MaxValue)
                return DataTypes.UInt8;

            return ((short)value).GetFormatType();
        }
    }
}