using System;
using System.Text;

namespace ProGaudi.Tarantool.Client.Utils
{
    public static class ByteUtils
    {
        public static string ToReadableString(ReadOnlySpan<byte> span)
        {
            var builder = new StringBuilder();
            var length = 80/3;
            for (var i = 0; i < span.Length; i++)
            {
                if (i%length == 0)
                    builder.AppendLine().Append("   ");
                else
                    builder.Append(" ");

                builder.AppendFormat((string) "{0:X2}", (object) span[i]);
            }

            return builder.ToString();
        }
    }
}
