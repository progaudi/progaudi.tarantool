using System.Collections.Generic;
using Xunit;

namespace MsgPack.Tests.MsgPackWriter
{
    public class Null : BaseWriterTest
    {
        [Fact]
        public void WriteNullString()
        {
            Writer.Write((string)null);
            AssertStreamContent(new byte[] { 0xc0 });
        }

        [Fact]
        public void WriteNullByteArray()
        {
            Writer.WriteByteArray(null);
            AssertStreamContent(new byte[] { 0xc0 });
        }
        [Fact]
        public void WriteNullArray()
        {
            Writer.Write((int[])null);
            AssertStreamContent(new byte[] { 0xc0 });
        }
        [Fact]
        public void WriteNullMap()
        {
            Writer.Write((IDictionary<int,int>)null);
            AssertStreamContent(new byte[] { 0xc0 });
        }
    }
}