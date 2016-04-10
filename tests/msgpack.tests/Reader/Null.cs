using Shouldly;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class Null:BaseReaderTest
    {
        //[Fact]
        public void ReadString()
        {
            var reader = CreateReader(new byte[] { 0xc0 });
            reader.ReadString().ShouldBeNull();
        }

        //[Fact]
        public void ReadBinary()
        {
            var reader = CreateReader(new byte[] { 0xc0 });
            reader.ReadBinary().ShouldBeNull();
        }

        //[Fact]
        public void ReadArray()
        {
            var reader = CreateReader(new byte[] { 0xc0 });
            reader.ReadArray<int>().ShouldBeNull();
        }

        //[Fact]
        public void ReadDictionary()
        {
            var reader = CreateReader(new byte[] { 0xc0 });
            reader.ReadMap<int, int>().ShouldBeNull();
        }
    }
}