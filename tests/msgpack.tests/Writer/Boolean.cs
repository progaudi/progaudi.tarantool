using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Boolean
    {
        [Fact]
        public void False()
        {
            MsgPackSerializer.Serialize(false).ShouldBe(new[] {(byte) DataTypes.False});
        }

        [Fact]
        public void True()
        {
            MsgPackSerializer.Serialize(true).ShouldBe(new[] {(byte) DataTypes.True});
        }
    }
}