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
                {"int1", (ushort)50505},
                {"int2", (byte)50},
                {3.14f, 3.14},
                {(byte)42, (byte)42}
            };

            var bytes = MsgPackLite.Pack(initialDictionary);
            var result = MsgPackLite.Unpack(bytes) as Dictionary<object, object>;

            CompareDictionaries(expectedResult, result);
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
            var initialDictionary = new Dictionary<object, object>(ushort.MaxValue+1);
            for (uint i = 0; i < ushort.MaxValue; i++)
            {
                initialDictionary.Add(i.ToString(), i.ToString());
            }

            var bytes = MsgPackLite.Pack(initialDictionary);
            var result = MsgPackLite.Unpack(bytes) as Dictionary<object, object>;

            CompareDictionaries(initialDictionary, result);
        }

        private static void CompareDictionaries(Dictionary<object, object> expectedResult, Dictionary<object, object> result)
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