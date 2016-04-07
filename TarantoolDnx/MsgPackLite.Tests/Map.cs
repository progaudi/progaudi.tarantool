using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class Map
    {
        [TestMethod]
        public void TestMapUpTo15()
        {
            var initialDictionary = new Dictionary<object, object>()
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
                {"int1", 50505},
                {"int2", (byte) 50},
                {3.14f, 3.14},
                {(byte) 42, (byte) 42}
            };

            var bytes = MsgPackLite.Pack(initialDictionary);
            var result = MsgPackLite.Unpack(bytes) as Dictionary<object, object>;

            CompareDictionaries(expectedResult, result);
        }

        [TestMethod]
        public void TestMapPack()
        {
            var initialDictionary = new Dictionary<object, object>()
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

            var mapExpected = new[]
            {
                new byte[]
                {
                    136, 166, 97, 114, 114, 97, 121, 49, 147, 173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101,
                    49, 173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 50, 173, 97, 114, 114, 97, 121, 49,
                    95, 118, 97, 108, 117, 101, 51, 165, 98, 111, 111, 108, 49, 195, 167, 100, 111, 117, 98, 108, 101,
                    49, 203, 64, 73, 64, 0, 0, 0, 0, 0, 167, 100, 111, 117, 98, 108, 101, 50, 203, 64, 46, 102, 102, 102,
                    102, 102, 102, 164, 105, 110, 116, 49, 210, 0, 0, 197, 73, 164, 105, 110, 116, 50, 50, 202, 64, 72,
                    245, 195, 203, 64, 9, 30, 184, 81, 235, 133, 31, 42, 42,
                },
            };

            Helpers.AssertPackResultEqual(new[] {initialDictionary}, mapExpected);
        }

        [TestMethod]
        public void TestMapUpToUInt16()
        {
            var initialDictionary = new Dictionary<object, object>(ushort.MaxValue);
            for (int i = 0; i < ushort.MaxValue; i++)
            {
                initialDictionary.Add(i.ToString(), i.ToString());
            }

            var bytes = MsgPackLite.Pack(initialDictionary);
            var result = MsgPackLite.Unpack(bytes) as Dictionary<object, object>;

            CompareDictionaries(initialDictionary, result);
        }

        [TestMethod]
        public void TestMapUpToUInt32()
        {
            //Can't create dictionary uint.MaxValue capacity - OutOfMemoryException
            var initialDictionary = new Dictionary<object, object>(ushort.MaxValue + 1);
            for (uint i = 0; i < ushort.MaxValue; i++)
            {
                initialDictionary.Add(i.ToString(), i.ToString());
            }

            var bytes = MsgPackLite.Pack(initialDictionary);
            var result = MsgPackLite.Unpack(bytes) as Dictionary<object, object>;

            CompareDictionaries(initialDictionary, result);
        }

        private static void CompareDictionaries(Dictionary<object, object> expectedResult,
            IReadOnlyDictionary<object, object> result)
        {
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
    }
}