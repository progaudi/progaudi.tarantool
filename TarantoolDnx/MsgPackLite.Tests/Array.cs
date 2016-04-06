using System.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Array
    {
        [TestMethod]
        public void TestArrayUpTo15()
        {
            var tests = new object[]
            {
                (float) 0,
                (float) 50505,
                float.NaN,
                float.MaxValue,
                float.MinValue,
                float.PositiveInfinity,
                float.NegativeInfinity,
                float.Epsilon,
            };


            var bytes = MsgPackLite.Pack(tests);
            var result = MsgPackLite.Unpack(bytes) as IList;

            Assert.IsTrue(result != null);
            Assert.IsTrue(tests.Length == result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(tests[i], result[i]);
            }
        }

        [TestMethod]
        public void TestArrayUpToIntInt16()
        {
            var tests = new object[ushort.MaxValue];

            for (int i = 0; i < ushort.MaxValue; i++)
            {
                tests[i] = (float)i;
            }

            var bytes = MsgPackLite.Pack(tests);
            var result = MsgPackLite.Unpack(bytes) as IList;

            Assert.IsTrue(result != null);
            Assert.IsTrue(tests.Length == result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(tests[i], result[i]);
            }
        }

        [TestMethod]    
        public void TestArrayUpToIntInt32()
        {
            var tests = new object[ushort.MaxValue + 1];

            for (int i = 0; i < ushort.MaxValue + 1; i++)
            {
                tests[i] = (float)i;
            }

            var bytes = MsgPackLite.Pack(tests);
            var result = MsgPackLite.Unpack(bytes) as IList;

            Assert.IsTrue(result != null);
            Assert.IsTrue(tests.Length == result.Count);

            for (int i = 0; i < result.Count; i++)
            {
                Assert.AreEqual(tests[i], result[i]);
            }
        }
    }
}