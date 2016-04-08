using System.IO;
using MsgPackLite.Interfaces;

namespace MsgPack.Tests.MsgPackReader
{
    public class BaseReaderTest
    {
        protected IMsgPackReader CreateReader(byte[] data)
        {
            var stream = new MemoryStream(data);
            return new MsgPackLite.MsgPackReader(stream);
        }
    }
}