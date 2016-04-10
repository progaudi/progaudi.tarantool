namespace TarantoolDnx.MsgPack.Tests.Writer
{
    //public class Double
    //{
    //    [TestMethod]
    //    public void TestDouble()
    //    {
    //        var tests = new[]
    //        {
    //            0d,
    //            1d,
    //            -1d,
    //            224d,
    //            256d,
    //            65530d,
    //            65540d,
    //            double.NaN,
    //            double.MaxValue,
    //            double.MinValue,
    //            double.PositiveInfinity,
    //            double.NegativeInfinity
    //        };
    //        foreach (var value in tests)
    //        {
    //            Assert.AreEqual(value, MsgPackLite.Unpack(MsgPackLite.Pack(value)));
    //        }
    //    }

    //    [TestMethod]
    //    public void TestDoublePack()
    //    {
    //        var tests = new[]
    //        {
    //            0d,
    //            1d,
    //            -1d,
    //            224d,
    //            256d,
    //            65530d,
    //            65540d,
    //            double.NaN,
    //            double.MaxValue,
    //            double.MinValue,
    //            double.PositiveInfinity,
    //            double.NegativeInfinity
    //        };

    //        var doubleExpected = new[]
    //        {
    //            new byte[] {203, 0, 0, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 63, 240, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 191, 240, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 64, 108, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 64, 112, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 64, 239, 255, 64, 0, 0, 0, 0,},
    //            new byte[] {203, 64, 240, 0, 64, 0, 0, 0, 0,},
    //            new byte[] {203, 255, 248, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 127, 239, 255, 255, 255, 255, 255, 255,},
    //            new byte[] {203, 255, 239, 255, 255, 255, 255, 255, 255,},
    //            new byte[] {203, 127, 240, 0, 0, 0, 0, 0, 0,},
    //            new byte[] {203, 255, 240, 0, 0, 0, 0, 0, 0,},
    //        };

    //        Helpers.AssertPackResultEqual(tests, doubleExpected);
    //    }

    //    [TestMethod]
    //    public void TestFloat()
    //    {
    //        var tests = new[]
    //        {
    //            0,
    //            50505,
    //            float.NaN,
    //            float.MaxValue,
    //            float.MinValue,
    //            float.PositiveInfinity,
    //            float.NegativeInfinity,
    //            float.Epsilon
    //        };
    //        foreach (var value in tests)
    //        {
    //            Assert.AreEqual(value, MsgPackLite.Unpack(MsgPackLite.Pack(value)));
    //        }
    //    }
    //}
}