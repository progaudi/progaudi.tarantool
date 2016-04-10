using System.Collections.Generic;
using Shouldly;
using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class Null
    {
        [Fact]
        public void WriteNullString()
        {
            MsgPackConverter.Convert((string)null).ShouldBe(new [] { (byte)DataTypes.Null });
        }

        [Fact]
        public void WriteNullByteArray()
        {
            MsgPackConverter.Convert((byte[])null).ShouldBe(new [] { (byte)DataTypes.Null });
        }

        [Fact]
        public void WriteNullArray()
        {
            MsgPackConverter.Convert((int[])null).ShouldBe(new [] { (byte)DataTypes.Null });
        }

        [Fact]
        public void WriteNullMap()
        {
            MsgPackConverter.Convert((IDictionary<int, int>)null).ShouldBe(new [] { (byte)DataTypes.Null });
        }
    }
}