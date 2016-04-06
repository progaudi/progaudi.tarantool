using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Long
    {
        [TestMethod]
        public void TestLong()
        {
            var tests = new Dictionary<long, object>()
            {
                {0, (byte) 0},

                {1, (byte) 1},
                {-1, (sbyte) -1},

                {sbyte.MinValue, (short) sbyte.MinValue},
                {sbyte.MaxValue, (byte) sbyte.MaxValue},

                {byte.MaxValue, byte.MaxValue},

                {ushort.MaxValue, ushort.MaxValue},

                {int.MaxValue, (uint) int.MaxValue},
                {int.MinValue, (long) int.MinValue},

                {long.MaxValue, (ulong) long.MaxValue},
                {long.MinValue, long.MinValue},
            };

            foreach (var value in tests)
            {
                Assert.AreEqual(value.Value, MsgPackLite.Unpack(MsgPackLite.Pack(value.Key)));
            }
        }

        [TestMethod]
        public void TestUlong()
        {
            var tests = new Dictionary<ulong, object>()
            {
                {0, (byte) 0},
                {1, (byte) 1},
                {byte.MaxValue, byte.MaxValue},
                {ushort.MaxValue, ushort.MaxValue},
                {int.MaxValue, (uint) int.MaxValue},
                {long.MaxValue, (ulong) long.MaxValue},
                {ulong.MaxValue, ulong.MaxValue}
            };

            foreach (var value in tests)
            {
                Assert.AreEqual(value.Value, MsgPackLite.Unpack(MsgPackLite.Pack(value.Key)));
            }
        }
    }
}