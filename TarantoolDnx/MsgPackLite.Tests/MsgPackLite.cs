using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
                Assert.AreEqual(value,
                    MsgPackLite.Unpack(MsgPackLite.Pack(value), MsgPackLite.OPTION_UNPACK_RAW_AS_STRING));
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
        public void TestMap()
        {
            var intialDictionary = new Dictionary<object, object>()
            {
                {
                    "array1", new object[] 
                    {
                        "array1_value1",
                        "array1_value2",
                        "array1_value3",
                    }
                },
                {"bool1", true},
                {"double1", 50.5},
                {"double2", 15.2},
                {"int1", 50505},
                {"int2", 50},
                {3.14f, 3.14},
                {42, 42}
            };

            var expectedResult = new Dictionary<object, object>()
            {
                {
                    "array1", new object[]
                    {
                        "array1_value1",
                        "array1_value2",
                        "array1_value3",
                    }
                },
                {"bool1", true},
                {"double1", 50.5},
                {"double2", 15.2},
                {"int1", (ushort)50505},
                {"int2", (byte)50},
                {3.14f, 3.14},
                {(byte)42, (byte)42}
            };

            var bytes = MsgPackLite.Pack(intialDictionary);
            var result = MsgPackLite.Unpack(bytes, MsgPackLite.OPTION_UNPACK_RAW_AS_STRING) as Dictionary<object, object>;

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedResult.Count, result.Count);

            foreach (var key in expectedResult.Keys)
            {
                Assert.IsTrue(result.ContainsKey(key));

                var value = expectedResult[key];
                var resultValue = result[key];

                if (value is IEnumerable<object>)
                {
                    Assert.IsTrue((value as IEnumerable<object>).SequenceEqual(resultValue as IEnumerable<object>));
                }
                else
                {
                   Assert.AreEqual(value, resultValue);
                }
            }
        }

        [TestMethod]
        public void TestArray()
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

        [TestMethod]
        public void TestBoolean()
        {
            var trueValue = MsgPackLite.Unpack(MsgPackLite.Pack(true));
            var falseValue = MsgPackLite.Unpack(MsgPackLite.Pack(false));

            Assert.IsTrue((bool) trueValue);
            Assert.IsFalse((bool) falseValue);
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