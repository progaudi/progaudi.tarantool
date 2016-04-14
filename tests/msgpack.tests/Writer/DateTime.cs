using System;
using System.Collections.Generic;

using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class DateTime1
    {
        [Fact]
        public void TestDateTime()
        {
            var tests = new List<KeyValuePair<System.DateTime, byte[]>>()
            {
                new KeyValuePair<DateTime, byte[]>(DateTime.MinValue, new byte[] {211, 247, 96, 128, 10, 8, 74, 128, 0,}),
                new KeyValuePair<DateTime, byte[]>(DateTime.MaxValue, new byte[] {211, 35, 42, 168, 102, 215, 52, 135, 255,}),
                new KeyValuePair<DateTime, byte[]>(new DateTime(2015, 11, 17), new byte[] {211, 0, 51, 110, 210, 236, 93, 200, 0,}),
                new KeyValuePair<DateTime, byte[]>(new DateTime(1, 2, 3, 4, 5, 6, DateTimeKind.Utc), new byte[] {211, 247, 96, 154, 26, 189, 97, 197, 0,}),
            };

            foreach (var test in tests)
            {
                MsgPackConverter.Serialize(test.Key).ShouldBe(test.Value);
            }
        }

        [Fact]
        public void TestDateTimeOffset()
        {
            var tests = new List<KeyValuePair<System.DateTimeOffset, byte[]>>()
            {
                new KeyValuePair<DateTimeOffset, byte[]>(DateTimeOffset.MinValue, new byte[] {211, 247, 96, 128, 10, 8, 74, 128, 0,}),
                new KeyValuePair<DateTimeOffset, byte[]>(DateTimeOffset.MaxValue, new byte[] {211, 35, 42, 168, 127, 252, 129, 191, 255,}),
                new KeyValuePair<DateTimeOffset, byte[]>(new DateTimeOffset(2015, 11, 17, 0, 0, 0, TimeSpan.Zero), new byte[] {211, 0, 51, 110, 236, 17, 171, 0, 0,}),
                new KeyValuePair<DateTimeOffset, byte[]>(new DateTimeOffset(1, 2, 3, 4, 5, 6, TimeSpan.Zero), new byte[] {211, 247, 96, 154, 26, 189, 97, 197, 0,}),
                new KeyValuePair<DateTimeOffset, byte[]>(new DateTimeOffset(1, 2, 3, 4, 5, 6, TimeSpan.FromHours(12)), new byte[] {211, 247, 96, 153, 182, 40, 44, 229, 0,}),
                new KeyValuePair<DateTimeOffset, byte[]>(new DateTimeOffset(1, 2, 3, 4, 5, 6, TimeSpan.FromMinutes(361)), new byte[] {211, 247, 96, 153, 232, 79, 4, 15, 0,}),
            };

            foreach (var test in tests)
            {
                MsgPackConverter.Serialize(test.Key).ShouldBe(test.Value);
            }
        }
    }
}