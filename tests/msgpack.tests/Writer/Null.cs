using System.Collections.Generic;

using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Null
    {
        [Fact]
        public void WriteNullArray()
        {
            MsgPackSerializer.Serialize((int[]) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullByteArray()
        {
            MsgPackSerializer.Serialize((byte[]) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullMap()
        {
            MsgPackSerializer.Serialize((IDictionary<int, int>) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }

        [Fact]
        public void WriteNullString()
        {
            MsgPackSerializer.Serialize((string) null).ShouldBe(new[] {(byte) DataTypes.Null});
        }
    }
}