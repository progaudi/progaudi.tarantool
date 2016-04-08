using Xunit;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class Null:BaseReaderTest
    {
        [Fact]
        public void ReadString()
        {
            var msgPackReader = CreateReader(new byte[] { 0xc0 });
            Assert.Null(msgPackReader.ReadString());
        }

        [Fact]
        public void ReadBinary()
        {
            var msgPackReader = CreateReader(new byte[] { 0xc0 });
            Assert.Null(msgPackReader.ReadBinary());
        }

        [Fact]
        public void ReadArray()
        {
            var msgPackReader = CreateReader(new byte[] { 0xc0 });
            Assert.Null(msgPackReader.ReadArray<int>());
        }

        [Fact]
        public void ReadDictionary()
        {
            var msgPackReader = CreateReader(new byte[] { 0xc0 });
            Assert.Null(msgPackReader.ReadMap<int,int>());
        }
    }
}