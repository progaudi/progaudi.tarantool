using System;
using System.Collections.Generic;

using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class DateTime
    {
        [Fact]
        public void TestDateTime()
        {
            var tests = new List<KeyValuePair<byte[], System.DateTime>>()
            {
                new KeyValuePair<byte[], System.DateTime>(new byte[] {0,}, System.DateTime.MinValue),
                new KeyValuePair<byte[], System.DateTime>(new byte[] {211, 43, 202, 40, 117, 244, 55, 63, 255,}, System.DateTime.MaxValue),
                new KeyValuePair<byte[], System.DateTime>(new byte[] {0,}, new System.DateTime()),
                new KeyValuePair<byte[], System.DateTime>(new byte[] {0,}, new System.DateTime(1, 1, 1)),
                new KeyValuePair<byte[], System.DateTime>(new byte[] {211, 8, 210, 238, 226, 9, 96, 128, 0,}, new System.DateTime(2015, 11, 17)),
                new KeyValuePair<byte[], System.DateTime>(new byte[] {211, 0, 0, 26, 16, 181, 23, 69, 0,}, new System.DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
            };

            foreach (var test in tests)
            {
                MsgPackConverter.Deserialize<System.DateTime>(test.Key).ShouldBe(test.Value);
            }
        }
    }
}