using System;
using System.Globalization;

namespace ProGaudi.Tarantool.Client.Model
{
    internal class Format
    {
        internal static string ToString(double value)
        {
            if (double.IsInfinity(value))
            {
                if (double.IsPositiveInfinity(value))
                    return "+inf";
                if (double.IsNegativeInfinity(value))
                    return "-inf";
            }
            return value.ToString("G17", (IFormatProvider)NumberFormatInfo.InvariantInfo);
        }

        internal static bool TryParseDouble(string s, out double value)
        {
            if (string.IsNullOrEmpty(s))
            {
                value = 0.0;
                return false;
            }
            if (s.Length == 1 && (int)s[0] >= 48 && (int)s[0] <= 57)
            {
                value = (double)((int)s[0] - 48);
                return true;
            }
            if (string.Equals("+inf", s, StringComparison.OrdinalIgnoreCase) || string.Equals("inf", s, StringComparison.OrdinalIgnoreCase))
            {
                value = double.PositiveInfinity;
                return true;
            }
            if (!string.Equals("-inf", s, StringComparison.OrdinalIgnoreCase))
                return double.TryParse(s, NumberStyles.Any, (IFormatProvider)NumberFormatInfo.InvariantInfo, out value);
            value = double.NegativeInfinity;
            return true;
        }
    }
}
