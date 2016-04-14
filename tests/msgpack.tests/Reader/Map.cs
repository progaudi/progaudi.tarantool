using Shouldly;
using System.Collections.Generic;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class Map
    {
        [Fact]
        public void ComplexDictionary()
        {
            var tests = new Dictionary<object, object>
            {
                {
                    "array1",
                    new object[]
                    {
                        "array1_value1",
                        "array1_value2",
                        "array1_value3"
                    }
                },
                {"bool1", true},
                {"double1", 50.5},
                {"double2", 15.2},
                {"int1", 50505},
                {"int2", 50},
                {3.14f, 3.14},
                {42, 42},
                {new Dictionary<int, int> {{1, 2}}, null},
                {new[] {1, 2}, null}
            };

            var data = new byte[]
            {
                138,
                166, 97, 114, 114, 97, 121, 49,
                    147,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 49,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 50,
                    173, 97, 114, 114, 97, 121, 49, 95, 118, 97, 108, 117, 101, 51,
                165, 98, 111, 111, 108, 49, 195,
                167, 100, 111, 117, 98, 108, 101, 49, 203, 64, 73, 64, 0, 0, 0, 0, 0,
                167, 100, 111, 117, 98, 108, 101, 50, 203, 64, 46, 102, 102, 102, 102, 102, 102,
                164, 105, 110, 116, 49, 205, 197, 73,
                164, 105, 110, 116, 50, 50,
                202, 64, 72, 245, 195, 203, 64, 9, 30, 184, 81, 235, 133, 31,
                42, 42,
                129, 1, 2, 192,
                146, 1, 2, 192
            };

            var settings = new MsgPackContext();
            settings.RegisterConverter(new TestReflectionConverter());
            MsgPackConverter.Serialize(tests, settings).ShouldBe(data);
        }

        [Fact]
        public void SimpleDictionary()
        {
            var test = new Dictionary<int, string>
            {
                {1, "a"},
                {2, "b"},
                {3, "c"},
                {4, "d"},
                {5, "e"}
            };

            var bytes = new byte[]
            {
                133,
                1, 161, 97,
                2, 161, 98,
                3, 161, 99,
                4, 161, 100,
                5, 161, 101
            };

            MsgPackConverter.Deserialize<Dictionary<int, string>>(bytes).ShouldBe(test);
        }

        [Fact]
        public void NonFixedDictionary()
        {
            var bytes = new byte[]
            {
                0xde,
                0x00,
                0x14,

                0x01, 0xa1, 0x61,
                0x02, 0xa1, 0x62,
                0x03, 0xa1, 0x63,
                0x04, 0xa1, 0x64,
                0x05, 0xa1, 0x65,

                0x0b, 0xa1, 0x61,
                0x0c, 0xa1, 0x62,
                0x0d, 0xa1, 0x63,
                0x0e, 0xa1, 0x64,
                0x0f, 0xa1, 0x65,

                0x15, 0xa1, 0x61,
                0x16, 0xa1, 0x62,
                0x17, 0xa1, 0x63,
                0x18, 0xa1, 0x64,
                0x19, 0xa1, 0x65,

                0x1f, 0xa1, 0x61,
                0x20, 0xa1, 0x62,
                0x21, 0xa1, 0x63,
                0x22, 0xa1, 0x64,
                0x23, 0xa1, 0x65,
            };

            var test = new Dictionary<int, string>
            {
                {1, "a"},
                {2, "b"},
                {3, "c"},
                {4, "d"},
                {5, "e"},

                {11, "a"},
                {12, "b"},
                {13, "c"},
                {14, "d"},
                {15, "e"},

                {21, "a"},
                {22, "b"},
                {23, "c"},
                {24, "d"},
                {25, "e"},

                {31, "a"},
                {32, "b"},
                {33, "c"},
                {34, "d"},
                {35, "e"},
            };

            MsgPackConverter.Deserialize<Dictionary<int, string>>(bytes).ShouldBe(test);
        }
    }
}