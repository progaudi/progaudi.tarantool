using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Tests
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
        public void TestNull()
        {
            Assert.AreEqual(null, MsgPackLite.Unpack(MsgPackLite.Pack(null)));
        }
        [TestMethod]
        public void TestString()
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
                Assert.AreEqual(value, MsgPackLite.Unpack(MsgPackLite.Pack(value), MsgPackLite.OPTION_UNPACK_RAW_AS_STRING));
            }
        }

        [TestMethod]
        public void TestLong()
        {
            var tests = new Dictionary<long, object>()
            {
                {0, (byte) 0},

                {1, (byte) 1},
                {-1, (sbyte) -1},

                {sbyte.MinValue, (short)sbyte.MinValue},
                {sbyte.MaxValue, (byte)sbyte.MaxValue},

                {byte.MaxValue, byte.MaxValue},

                {ushort.MaxValue, ushort.MaxValue},

                {int.MaxValue, (uint) int.MaxValue},
                {int.MinValue, (long)int.MinValue},

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