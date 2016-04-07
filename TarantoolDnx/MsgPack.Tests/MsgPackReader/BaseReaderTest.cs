using System.IO;
using MsgPackLite;

namespace MsgPack.Tests.MsgPackReader
{
    public class BaseReaderTest
    {
        protected IMsgPackReader CreateReader(byte[] data)
        {
            using (var stream = new MemoryStream(data))
            {
                return new MsgPackLite.MsgPackReader(stream);
            }
        }
    }
}