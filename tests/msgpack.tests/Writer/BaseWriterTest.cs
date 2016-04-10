using System.IO;
using Shouldly;
using TarantoolDnx.MsgPack.Interfaces;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class BaseWriterTest
    {
        protected readonly IMsgPackWriter Writer;
        private readonly MemoryStream _innerStream = new MemoryStream();

        public BaseWriterTest()
        {
        }

        protected void BufferShouldBe(byte[] expectedBytes)
        {
            var actualBytes = _innerStream.ToArray();
            actualBytes.ShouldBe(expectedBytes);
        }
    }
}