using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
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
            var crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            return data;
        }

        public static void AssertPackResultEqual<T>(T[] tests, byte[][] doubleExpected)
        {
            for (var i = 0; i < tests.Length; i++)
            {
                var resultBytes = MsgPackLite.Pack(tests[i]);
                var expectedBytes = doubleExpected[i];
                for (var j = 0; j < resultBytes.Length; j++)
                {
                    Assert.AreEqual(expectedBytes[j], resultBytes[j]);
                }
            }
        }
    }
}
