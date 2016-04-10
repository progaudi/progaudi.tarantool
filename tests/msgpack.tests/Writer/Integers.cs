using Shouldly;
using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Integers
    {
        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(-1, new byte[] { 0xff })]
        [InlineData(sbyte.MinValue, new byte[] { 208, 128 })]
        [InlineData(sbyte.MaxValue, new byte[] { 127 })]
        [InlineData(short.MinValue, new byte[] { 209, 128, 0 })]
        [InlineData(short.MaxValue, new byte[] { 209, 127, 0xff })]
        [InlineData(int.MinValue, new byte[] { 210, 128, 0, 0, 0 })]
        [InlineData(int.MaxValue, new byte[] { 210, 127, 0xff, 0xff, 0xff })]
        [InlineData(long.MaxValue, new byte[] { 211, 127, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(long.MinValue, new byte[] { 211, 128, 0, 0, 0, 0, 0, 0, 0 })]
        public void TestSigned(long number, byte[] data)
        {
            MsgPackConverter.Convert(number).ShouldBe(data);
        }

        [Theory]
        [InlineData(0, new byte[] { 0x00 })]
        [InlineData(1, new byte[] { 1 })]
        [InlineData(byte.MaxValue, new byte[] { 0xcc, 0xff })]
        [InlineData(ushort.MaxValue, new byte[] { 0xcd, 0xff, 0xff })]
        [InlineData(uint.MaxValue, new byte[] { 0xce, 0xff, 0xff, 0xff, 0xff })]
        [InlineData(ulong.MaxValue, new byte[] { 0xcf, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff, 0xff })]
        public void TetsUnsigned(ulong number, byte[] data)
        {
            MsgPackConverter.Convert(number).ShouldBe(data);
        }
    }
}