using System.Collections.Generic;

using Shouldly;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Null
    {
        [Fact]
        public void WriteNullArray()
        {
            MsgPackConverter.Serialize((int[]) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullByteArray()
        {
            MsgPackConverter.Serialize((byte[]) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullMap()
        {
            MsgPackConverter.Serialize((IDictionary<int, int>) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullString()
        {
            MsgPackConverter.Serialize((string) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }
    }
}