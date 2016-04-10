using System.IO;
using TarantoolDnx.MsgPack.Interfaces;

namespace TarantoolDnx.MsgPack.Tests.Reader
{
    public class BaseReaderTest
    {
        protected IMsgPackReader CreateReader(byte[] data)
        {
            var stream = new MemoryStream(data);
            return null;
        }
    }
}