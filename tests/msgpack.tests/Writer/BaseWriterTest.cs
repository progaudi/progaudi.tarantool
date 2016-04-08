using System.IO;
using TarantoolDnx.MsgPack.Interfaces;
using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Writer
{
    public class BaseWriterTest
    {
        protected readonly IMsgPackWriter Writer;
        private readonly Stream _innerStream = new MemoryStream();

        public BaseWriterTest()
        {
            Writer = new MsgPackWriter(_innerStream);
        }

        protected void AssertStreamContent(byte[] expectedBytes)
        {
            var actualBytes = ReadFully(_innerStream);
            Assert.Equal(actualBytes, expectedBytes);
        }

        private static byte[] ReadFully(Stream input)
        {
            input.Position = 0;
            byte[] buffer = new byte[16 * 1024];
            using (MemoryStream ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }
                return ms.ToArray();
            }
        }
    }
}