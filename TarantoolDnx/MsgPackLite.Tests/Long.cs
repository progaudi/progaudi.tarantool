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
            var tests = new Dictionary<object, object>()
            {
                {0, (byte) 0},

                {1, (byte) 1},
                {-1, (sbyte) -1},

                {sbyte.MinValue, sbyte.MinValue},
                {sbyte.MaxValue, (byte) sbyte.MaxValue},

                {byte.MaxValue, byte.MaxValue},

                {ushort.MaxValue, ushort.MaxValue},

                {int.MaxValue, int.MaxValue},
                {int.MinValue, int.MinValue},

                {long.MaxValue,long.MaxValue},
                {long.MinValue, long.MinValue},
            };

            foreach (var value in tests)
            {
                Assert.AreEqual(value.Value, MsgPackLite.Unpack(MsgPackLite.Pack(value.Key)));
            }
        }

        [TestMethod]
        public void TestLongPack()
        {
            var tests = new object[]
            {
                0,
                1,
                -1,
                sbyte.MinValue,
                sbyte.MaxValue,
                byte.MaxValue,
                short.MinValue,
                short.MaxValue,
                int.MinValue,
                int.MaxValue,
                long.MaxValue,
                long.MinValue,
            };

            var longExpected = new[]
            {
                new byte[] {0,},
                new byte[] {1,},
                new byte[] {255,},
                new byte[] {208, 128,},
                new byte[] {127,},
                new byte[] {204, 255,},
                new byte[] {209, 128, 0,},
                new byte[] {209, 127, 255,},
                new byte[] {210, 128, 0, 0, 0,},
                new byte[] {210, 127, 255, 255, 255,},
                new byte[] {211, 127, 255, 255, 255, 255, 255, 255, 255,},
                new byte[] {211, 128, 0, 0, 0, 0, 0, 0, 0,},
            };

            Helpers.AssertPackResultEqual(tests, longExpected);
        }

        [TestMethod]
        public void TestUlong()
        {
            var tests = new Dictionary<object, object>()
            {
                {0, (byte) 0},
                {1, (byte) 1},
                {byte.MaxValue, byte.MaxValue},
                {ushort.MaxValue, ushort.MaxValue},
                {int.MaxValue, int.MaxValue},
                {long.MaxValue, long.MaxValue},
                {ulong.MaxValue, ulong.MaxValue}
            };

            foreach (var value in tests)
            {
                Assert.AreEqual(value.Value, MsgPackLite.Unpack(MsgPackLite.Pack(value.Key)));
            }
        }

        [TestMethod]
        public void TestULongPack()
        {
            var tests = new object[]
            {
                ulong.MaxValue,
                ulong.MinValue,
            };

            var ulongExpected = new[]
            {
                new byte[] {207, 255, 255, 255, 255, 255, 255, 255, 255,},
                new byte[] {0},
            };

            Helpers.AssertPackResultEqual(tests, ulongExpected);
        }
    }
}