using Shouldly;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Boolean
    {
        [Fact]
        public void False()
        {
            MsgPackConverter.Serialize(false).ShouldBe(new[] {(byte) DataTypes.False});
        }

        [Fact]
        public void True()
        {
            MsgPackConverter.Serialize(true).ShouldBe(new[] {(byte) DataTypes.True});
        }
    }
}