using Shouldly;
using System.Collections.Generic;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class Array
    {
        [Fact]
        public void SimpleArray()
        {
            var tests = new[]
            {
                "a",
                "b",
                "c",
                "d",
                "e"
            };

            var bytes = new byte[]
            {
                149,
                161, 97,
                161, 98,
                161, 99,
                161, 100,
                161, 101
            };

            MsgPackSerializer.Deserialize<string[]>(bytes).ShouldBe(tests);
        }

        [Fact]
        public void TestArrayPack()
        {
            var expected = new object[]
            {
                0,
                50505,
                float.NaN,
                float.MaxValue,
                new[] {true, false, true},
                null,
                new Dictionary<object, object> {{"Ball", "Soccer"}}
            };

            var data = new byte[]
            {
                151,
                0,
                205, 197, 73,
                202, 255, 192, 0, 0,
                202, 127, 127, 255, 255,
                147,
                    195,
                    194,
                    195,
                192,
                129,
                    164, 66, 97, 108, 108, 166, 83, 111, 99, 99, 101, 114
            };

            var settings = new MsgPackContext();
            settings.RegisterConverter(new TestReflectionConverter());

            MsgPackSerializer.Deserialize<object[]>(data, settings).ShouldBe(expected);
        }

        [Fact]
        public void TestNonFixedArray()
        {
            var array = new[]
               {
                    1, 2, 3, 4, 5,
                    1, 2, 3, 4, 5,
                    1, 2, 3, 4, 5,
                    1, 2, 3, 4, 5,
                };

            var bytes = new byte[]
            {
                0xdc,
                0x00,
                0x14,

                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
                0x01, 0x02, 0x03, 0x04, 0x05,
            };

            MsgPackSerializer.Deserialize<int[]>(bytes).ShouldBe(array);
        }
    }
}