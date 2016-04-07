using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MsgPackLite.Tests
{
    [TestClass]
    public class String
    {
        [TestMethod]
        public void TestStringUpTo15()
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
                    MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }

        [TestMethod]
        public void TestStringPack()
        {
            var tests = new []
             {
                "asdf",
                "akjsdhflk123451235",
                string.Empty,
                "08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg08612364123065712639gr1h2j3grtk1h23kgfrt1hj2g3fjrgf1j2hg",
             };

            var stringsExpected = new[]
            {
                new byte[] {164, 97, 115, 100, 102,},
                new byte[] {178, 97, 107, 106, 115, 100, 104, 102, 108, 107, 49, 50, 51, 52, 53, 49, 50, 51, 53,},
                new byte[] {160,},
                new byte[]
                {
                    218, 1, 80, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114,
                    49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51, 48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49,
                    104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50, 51, 107, 103, 102, 114, 116, 49, 104, 106, 50,
                    103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104, 103, 48, 56, 54, 49, 50, 51, 54, 52, 49, 50, 51,
                    48, 54, 53, 55, 49, 50, 54, 51, 57, 103, 114, 49, 104, 50, 106, 51, 103, 114, 116, 107, 49, 104, 50,
                    51, 107, 103, 102, 114, 116, 49, 104, 106, 50, 103, 51, 102, 106, 114, 103, 102, 49, 106, 50, 104,
                    103,
                },
            };

            Helpers.AssertPackResultEqual(tests, stringsExpected);
        }

        [TestMethod]
        public void TestStringUptToInt16()
        {
            var tests = new string[ushort.MaxValue];

            for (var i = 0; i < ushort.MaxValue; i++)
            {
                tests[i] = i.ToString();
            }
            foreach (var value in tests)
            {
                Assert.AreEqual(value,
                    MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }

        [TestMethod]
        public void TestStringUptToInt32()
        {
            var tests = new string[ushort.MaxValue + 1];

            for (var i = 0; i < ushort.MaxValue + 1; i++)
            {
                tests[i] = i.ToString();
            }
            foreach (var value in tests)
            {
                Assert.AreEqual(value,
                    MsgPackLite.Unpack(MsgPackLite.Pack(value)));
            }
        }
    }
}