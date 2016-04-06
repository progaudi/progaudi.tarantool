using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Double
    {
        [TestMethod]
        public void TestDouble()
        {
            var tests = new[]
            {
                0d,
                1d,
                -1d,
                224d,
                256d,
                65530d,
                65540d,
                double.NaN,
                double.MaxValue,
                double.MinValue,
                double.PositiveInfinity,
                double.NegativeInfinity
            };
            foreach (var value in tests)
            {
                Assert.AreEqual(value, MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }

        [TestMethod]
        public void TestFloat()
        {
            var tests = new[]
            {
                0,
                50505,
                float.NaN,
                float.MaxValue,
                float.MinValue,
                float.PositiveInfinity,
                float.NegativeInfinity,
                float.Epsilon
            };
            foreach (var value in tests)
            {
                Assert.AreEqual(value, MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }
    }
}