using System;
using System.Buffers;
using System.Security.Cryptography;
using System.Text;

namespace ProGaudi.Tarantool.Client.Utils
{
    internal static class Sha1Utils
    {
        public static byte[] Hash(string str)
        {
            var bytes = str == null ? new byte[0] : Encoding.UTF8.GetBytes(str);

            return Hash(bytes);
        }

        public static byte[] Hash(byte[] bytes)
        {
            using (var sha1 = SHA1.Create())
            {
                return sha1.ComputeHash(bytes);
            }
        }

        public static byte[] Hash(ReadOnlySpan<byte> salt, ReadOnlySpan<byte> str)
        {
            var size = salt.Length + str.Length;
            using (var salted = MemoryPool<byte>.Shared.Rent(size))
            {
                var memory = salted.Memory;
                salt.CopyTo(memory.Span);
                str.CopyTo(memory.Span.Slice(salt.Length));

                return Hash(memory.Slice(0, size).ToArray());
            }
        }

        public static void Xor(ReadOnlySpan<byte> array1, ReadOnlySpan<byte> array2, Span<byte> destination)
        {
            if (array1.Length != array2.Length)
                throw new InvalidOperationException("array1 and array2 should be of same length");
            if (array1.Length != destination.Length)
                throw new InvalidOperationException("array1 and destination should be of same length");

            for (int i = 0; i < array1.Length; i++)
            {
                destination[i] = (byte)(array1[i] ^ array2[i]);
            }
        }
    }
}