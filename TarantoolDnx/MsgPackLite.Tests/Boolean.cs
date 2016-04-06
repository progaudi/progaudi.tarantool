using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Boolean
    {
        [TestMethod]
        public void TestBoolean()
        {
            var trueBytes = MsgPackLite.Pack(true);
            var falseBytes = MsgPackLite.Pack(false);
            var trueValue = MsgPackLite.Unpack(trueBytes);
            var falseValue = MsgPackLite.Unpack(falseBytes);

            Assert.IsTrue((bool)trueValue);
            Assert.IsFalse((bool)falseValue);

            Assert.IsTrue(trueBytes.SequenceEqual(new[] { (byte)0xc3 }));
            Assert.IsTrue(falseBytes.SequenceEqual(new[] { (byte)0xc2 }));
        }
    }
}