using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Binary
    {
        [TestMethod]
        public void TestBinary()
        {
            var tests = new[]
            {
                Helpers.GenerateBytesArray(8),
                Helpers.GenerateBytesArray(16),
                Helpers.GenerateBytesArray(32),
                Helpers.GenerateBytesArray(257),
                Helpers.GenerateBytesArray(65537)
            };
            foreach (var value in tests)
            {
                var result = MsgPackLite.Unpack(MsgPackLite.Pack(value));
                Assert.IsTrue(value.SequenceEqual(result as byte[]));
            }
        }

        [TestMethod]
        public void TestBinaryPack()
        {
            var tests = new[]
            {
                new byte[] {},
                new byte[] {123},
                new byte[] {123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,},
                new byte[]
                {
                    123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,
                    123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0,
                    123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123,
                    0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,
                    123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,
                },
            };

            var bytesExpected = new[]
            {
                new byte[] {196, 0,},
                new byte[] {196, 1, 123,},
                new byte[] {196, 12, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,},
                new byte[]
                {
                    196, 108, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,
                    123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0,
                    123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123,
                    0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,
                    123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123, 123, 0, 123,
                },
            };

            Helpers.AssertPackResultEqual(tests, bytesExpected);
        }
    }
}