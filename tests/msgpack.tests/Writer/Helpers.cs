using System;
using System.Text;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Helpers
    {
        public static string GenerateString(int size)
        {
            var chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            var data = GenerateBytesArray(size);
            var result = new StringBuilder(size);
            foreach (var b in data)
            {
                result.Append(chars[b % chars.Length]);
            }
            return result.ToString();
        }

        public static byte[] GenerateBytesArray(int size)
        {
            var data = new byte[size];
            var crypto = new Random();
            crypto.NextBytes(data);
            return data;
        }
    }
}