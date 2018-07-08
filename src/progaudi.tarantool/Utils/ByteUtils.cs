using System;
using System.Buffers;
using System.Buffers.Text;
using System.Linq;
using System.Text;

namespace ProGaudi.Tarantool.Client.Utils
{
    public static class ByteUtils
    {
        public static string ToReadableString(this byte[] bytes)
        {
            return string.Join(" ", bytes.Select(b => b.ToString("X2")));
        }

        public static string ToReadableString(this ArraySegment<byte> bytes)
        {
            return string.Join(" ", bytes.Select(b => b.ToString("X2")));
        }

        public static string ToReadableString(this ReadOnlySequence<byte> bytes)
        {
            var output = new StringBuilder((int) (bytes.Length * 3));

            foreach (var b in bytes)
            {
                var s = b.Span;
                for (var i = 0; i < s.Length; i++)
                    output.Append(s[i].ToString("X2"));
            }

            return output.ToString();
        }
    }
}
