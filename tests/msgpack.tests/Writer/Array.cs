using System.Collections;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    //public class Array
    //{
    //    public void TestArrayUpTo15()
    //    {
    //        var tests = new object[]
    //        {
    //            (float) 0,
    //            (float) 50505,
    //            float.NaN,
    //            float.MaxValue,
    //            float.MinValue,
    //            float.PositiveInfinity,
    //            float.NegativeInfinity,
    //            float.Epsilon,
    //        };

    //        var bytes = MsgPackLite.Pack(tests);
    //        var result = MsgPackLite.Unpack(bytes) as IList;

    //        Assert.IsTrue(result != null);
    //        Assert.IsTrue(tests.Length == result.Count);

    //        for (var i = 0; i < result.Count; i++)
    //        {
    //            Assert.AreEqual(tests[i], result[i]);
    //        }
    //    }

    //    [TestMethod]
    //    public void TestArrayPack()
    //    {
    //        var tests = new object[]
    //        {
    //            (float) 0,
    //            (float) 50505,
    //            float.NaN,
    //            float.MaxValue,
    //            float.MinValue,
    //            float.PositiveInfinity,
    //            float.NegativeInfinity,
    //            float.Epsilon,
    //        };

    //        var arrayExpected = new[]
    //        {
    //            new byte[]
    //            {
    //                152, 202, 0, 0, 0, 0, 202, 71, 69, 73, 0, 202, 255, 192, 0, 0, 202, 127, 127, 255, 255, 202, 255, 127,
    //                255, 255, 202, 127, 128, 0, 0, 202, 255, 128, 0, 0, 202, 0, 0, 0, 1,
    //            },
    //        };
    //        Helpers.AssertPackResultEqual(new[] {tests}, arrayExpected);
    //    }

    //    [TestMethod]
    //    public void TestArrayUpToIntInt16()
    //    {
    //        var tests = new object[ushort.MaxValue];

    //        for (int i = 0; i < ushort.MaxValue; i++)
    //        {
    //            tests[i] = (float)i;
    //        }

    //        var bytes = MsgPackLite.Pack(tests);
    //        var result = MsgPackLite.Unpack(bytes) as IList;

    //        Assert.IsTrue(result != null);
    //        Assert.IsTrue(tests.Length == result.Count);

    //        for (int i = 0; i < result.Count; i++)
    //        {
    //            Assert.AreEqual(tests[i], result[i]);
    //        }
    //    }

    //    [TestMethod]    
    //    public void TestArrayUpToIntInt32()
    //    {
    //        var tests = new object[ushort.MaxValue + 1];

    //        for (int i = 0; i < ushort.MaxValue + 1; i++)
    //        {
    //            tests[i] = (float)i;
    //        }

    //        var bytes = MsgPackLite.Pack(tests);
    //        var result = MsgPackLite.Unpack(bytes) as IList;

    //        Assert.IsTrue(result != null);
    //        Assert.IsTrue(tests.Length == result.Count);

    //        for (int i = 0; i < result.Count; i++)
    //        {
    //            Assert.AreEqual(tests[i], result[i]);
    //        }
    //    }
    //}
}