using Shouldly;
using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Boolean
    {
        [Fact]
        public void True()
        {
            MsgPackConverter.Convert(true).ShouldBe(new[] { (byte)DataTypes.True });
        }

        [Fact]
        public void False()
        {
            MsgPackConverter.Convert(false).ShouldBe(new[] { (byte)DataTypes.False });
        }
    }
}