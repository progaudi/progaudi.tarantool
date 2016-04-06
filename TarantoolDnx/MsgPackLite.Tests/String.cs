using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class String
    {
        [TestMethod]
        public void TestStringUpTo15()
        {
            var tests = new[]
            {
                Helpers.GenerateString(2),
                Helpers.GenerateString(8),
                Helpers.GenerateString(16),
                Helpers.GenerateString(32),
                Helpers.GenerateString(257),
                Helpers.GenerateString(65537)
            };
            foreach (var value in tests)
            {
                Assert.AreEqual(value,
                    MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }

        [TestMethod]
        public void TestStringUptToInt16()
        {
            var tests = new string[ushort.MaxValue];

            for (var i = 0; i < ushort.MaxValue; i++)
            {
                tests[i] = i.ToString();
            }
            foreach (var value in tests)
            {
                Assert.AreEqual(value,
                    MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }

        [TestMethod]
        public void TestStringUptToInt32()
        {
            var tests = new string[ushort.MaxValue + 1];

            for (var i = 0; i < ushort.MaxValue + 1; i++)
            {
                tests[i] = i.ToString();
            }
            foreach (var value in tests)
            {
                Assert.AreEqual(value,
                    MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }
    }
}