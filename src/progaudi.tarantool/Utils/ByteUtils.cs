using System.Buffers;
using System.Text;

namespace ProGaudi.Tarantool.Client.Utils
{
    public static class ByteUtils
    {
        public static string ToReadableString(ReadOnlySequence<byte> sequence)
        {
            var builder = new StringBuilder();
            var length = 80/3;
            foreach (var memory in sequence)
            {
                var span = memory.Span;

                for (var i = 0; i < span.Length; i++)
                {
                    if (i % length == 0)
                        builder.AppendLine().Append("   ");
                    else
                        builder.Append(" ");

                    builder.AppendFormat("{0:X2}", span[i]);
                }
            }

            return builder.ToString();
        }
    }
}
