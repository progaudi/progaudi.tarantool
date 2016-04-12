using System;

using Shouldly;

using TarantoolDnx.MsgPack.Convertes;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class FloatingPoint
    {
        [Theory]
        [InlineData(0, new byte[] {203, 0, 0, 0, 0, 0, 0, 0, 0})]
        [InlineData(1, new byte[] {203, 63, 240, 0, 0, 0, 0, 0, 0})]
        [InlineData(-1, new byte[] {203, 191, 240, 0, 0, 0, 0, 0, 0})]
        [InlineData(Math.E, new byte[] {203, 64, 5, 191, 10, 139, 20, 87, 105})]
        [InlineData(Math.PI, new byte[] {203, 64, 9, 33, 251, 84, 68, 45, 24})]
        [InlineData(224, new byte[] {203, 64, 108, 0, 0, 0, 0, 0, 0})]
        [InlineData(256, new byte[] {203, 64, 112, 0, 0, 0, 0, 0, 0})]
        [InlineData(65530, new byte[] {203, 64, 239, 255, 64, 0, 0, 0, 0})]
        [InlineData(65540, new byte[] {203, 64, 240, 0, 64, 0, 0, 0, 0})]
        [InlineData(double.NaN, new byte[] {203, 255, 248, 0, 0, 0, 0, 0, 0})]
        [InlineData(double.MaxValue, new byte[] {203, 127, 239, 255, 255, 255, 255, 255, 255})]
        [InlineData(double.MinValue, new byte[] {203, 255, 239, 255, 255, 255, 255, 255, 255})]
        [InlineData(double.PositiveInfinity, new byte[] {203, 127, 240, 0, 0, 0, 0, 0, 0})]
        [InlineData(double.NegativeInfinity, new byte[] {203, 255, 240, 0, 0, 0, 0, 0, 0})]
        public void TestDouble(double value, byte[] bytes)
        {
            MsgPackConverter.Deserialize<double>(bytes).ShouldBe(value);
        }

        [Theory]
        [InlineData(0, new byte[] {202, 0, 0, 0, 0})]
        [InlineData(1, new byte[] {202, 63, 128, 0, 0})]
        [InlineData(-1, new byte[] {202, 191, 128, 0, 0})]
        [InlineData(2.71828, new byte[] {202, 64, 45, 248, 77})]
        [InlineData(3.14159, new byte[] {202, 64, 73, 15, 208})]
        [InlineData(224, new byte[] {202, 67, 96, 0, 0})]
        [InlineData(256, new byte[] {202, 67, 128, 0, 0})]
        [InlineData(65530, new byte[] {202, 71, 127, 250, 0})]
        [InlineData(65540, new byte[] {202, 71, 128, 2, 0})]
        [InlineData(float.NaN, new byte[] {202, 255, 192, 0, 0})]
        [InlineData(float.MaxValue, new byte[] {202, 127, 127, 255, 255})]
        [InlineData(float.MinValue, new byte[] {202, 255, 127, 255, 255})]
        [InlineData(float.PositiveInfinity, new byte[] {202, 127, 128, 0, 0})]
        [InlineData(float.NegativeInfinity, new byte[] {202, 255, 128, 0, 0})]
        public void TestFloat(float value, byte[] bytes)
        {
            MsgPackConverter.Deserialize<float>(bytes).ShouldBe(value);
        }
    }
}