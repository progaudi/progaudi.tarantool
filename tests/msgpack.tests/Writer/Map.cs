using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Map
    {
        [Fact]
        public void TestMap()
        {
            var tests = new Dictionary<object, object>
            {
                {
                    "array1",
                    new object[]
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
                {42, 42},
                { new Dictionary<int, int> { { 1, 2 } }, null },
                { new [] { 1, 2 }, null }
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

            var settings = new MsgPackSettings();
            settings.RegisterConverter(new TestReflectionConverter());
            MsgPackConverter.Convert(tests, settings).ShouldBe(data);
        }
    }
}