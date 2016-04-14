using System;
using System.Collections.Generic;

using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class DateTime1
    {
        [Fact]
        public void TestDateTime()
        {
            var tests = new List<KeyValuePair<byte[], System.DateTime>>()
            {
                new KeyValuePair<byte[], DateTime>(new byte[] {211, 247, 96, 128, 10, 8, 74, 128, 0,}, DateTime.MinValue),
                new KeyValuePair<byte[], DateTime>(new byte[] {211, 35, 42, 168, 102, 215, 52, 135, 255,}, DateTime.MaxValue),
                new KeyValuePair<byte[], DateTime>(new byte[] {211, 0, 51, 110, 210, 236, 93, 200, 0,}, new DateTime(2015, 11, 17)),
                new KeyValuePair<byte[], DateTime>(new byte[] {211, 247, 96, 154, 26, 189, 97, 197, 0,}, new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc)),
            };

            foreach (var test in tests)
            {
                MsgPackConverter.Deserialize<System.DateTime>(test.Key).ShouldBe(test.Value.ToUniversalTime());
            }
        }

        [Fact]
        public void TestDateTimeOffset()
        {
            var tests = new List<KeyValuePair<byte[], System.DateTimeOffset>>()
            {
                new KeyValuePair<byte[], DateTimeOffset>(new byte[] {211, 247, 96, 128, 10, 8, 74, 128, 0,}, DateTimeOffset.MinValue),
                new KeyValuePair<byte[], DateTimeOffset>(new byte[] {211, 35, 42, 168, 127, 252, 129, 191, 255,}, DateTimeOffset.MaxValue),
                new KeyValuePair<byte[], DateTimeOffset>(new byte[] {211, 0, 51, 110, 236, 17, 171, 0, 0,}, new DateTimeOffset(2015, 11, 17, 0, 0, 0, TimeSpan.Zero)),
                new KeyValuePair<byte[], DateTimeOffset>(new byte[] {211, 247, 96, 154, 26, 189, 97, 197, 0,}, new DateTimeOffset(1, 2, 3, 4, 5, 6, TimeSpan.Zero)),
                new KeyValuePair<byte[], DateTimeOffset>(new byte[] {211, 247, 96, 153, 182, 40, 44, 229, 0,}, new DateTimeOffset(1, 2, 3, 4, 5, 6, TimeSpan.FromHours(12))),
                new KeyValuePair<byte[], DateTimeOffset>(new byte[] {211, 247, 96, 153, 232, 79, 4, 15, 0,}, new DateTimeOffset(1, 2, 3, 4, 5, 6, TimeSpan.FromMinutes(361))),
            };

            foreach (var test in tests)
            {
                MsgPackConverter.Deserialize<System.DateTimeOffset>(test.Key).ShouldBe(test.Value);
            }
        }
    }
}