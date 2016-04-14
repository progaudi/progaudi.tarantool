using System.Collections.Generic;

using Shouldly;

using TarantoolDnx.MsgPack.Converters;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class Null
    {
        [Fact]
        public void ReadNullArray()
        {
            MsgPackConverter.Deserialize<int[]>(new[] { (byte)DataTypes.Null }).ShouldBe(null);
        }

        [Fact]
        public void ReadNullByteArray()
        {
            MsgPackConverter.Deserialize<byte[]>(new[] { (byte)DataTypes.Null }).ShouldBe(null);
        }

        [Fact]
        public void ReadNullMap()
        {
            MsgPackConverter.Deserialize<Dictionary<int, int>>(new[] { (byte)DataTypes.Null }).ShouldBe(null);
        }

        [Fact]
        public void ReadNullString()
        {
            MsgPackConverter.Deserialize<string>(new[] { (byte)DataTypes.Null }).ShouldBe(null);
        }
    }
}