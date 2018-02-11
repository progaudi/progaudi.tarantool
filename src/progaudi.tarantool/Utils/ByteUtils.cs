using System;
using System.Linq;

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
    }
}
