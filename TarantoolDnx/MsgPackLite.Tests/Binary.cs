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
    }
}