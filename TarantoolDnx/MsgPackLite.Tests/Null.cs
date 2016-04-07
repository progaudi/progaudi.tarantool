using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Null
    {
        [TestMethod]
        public void TestNull()
        {
            var bytes = MsgPackLite.Pack(null);

            Assert.AreEqual(null, MsgPackLite.Unpack(bytes));
            Assert.IsTrue(bytes.SequenceEqual(new[] {(byte) 0xc0}));
        }
    }
}