using Shouldly;
using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class Boolean
    {
        [Fact]
        public void True()
        {
            MsgPackConverter.Deserialize<bool>(new[] { (byte)DataTypes.True }).ShouldBeTrue();
        }

        [Fact]
        public void False()
        {
            MsgPackConverter.Deserialize<bool>(new[] { (byte)DataTypes.False }).ShouldBeFalse();
        }
    }
}