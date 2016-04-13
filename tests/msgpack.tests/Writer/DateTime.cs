using System;
using System.Collections.Generic;
using System.Globalization;

using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class DateTime
    {
        [Fact]
        public void TestDateTime()
        {
            var tests = new List<KeyValuePair<System.DateTime, byte[]>>()
            {
                new KeyValuePair<System.DateTime, byte[]>(System.DateTime.MinValue, new byte[] {0,}),
                new KeyValuePair<System.DateTime, byte[]>(System.DateTime.MaxValue, new byte[] {211, 43, 202, 40, 117, 244, 55, 63, 255,}),
                new KeyValuePair<System.DateTime, byte[]>(new System.DateTime(), new byte[] {0,}),
                new KeyValuePair<System.DateTime, byte[]>(new System.DateTime(1, 1, 1), new byte[] {0,}),
                new KeyValuePair<System.DateTime, byte[]>(new System.DateTime(2015, 11, 17), new byte[] {211, 8, 210, 238, 226, 9, 96, 128, 0,}),
                new KeyValuePair<System.DateTime, byte[]>(new System.DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc), new byte[] {211, 0, 0, 26, 16, 181, 23, 69, 0,}),
            };

            foreach (var test in tests)
            {
                MsgPackConverter.Serialize(test.Key).ShouldBe(test.Value);
            }
        }
    }
}