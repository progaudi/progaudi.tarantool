using System.IO;
using System.Net;

using MsgPack.Light;

namespace Tarantool.Client
{
    public class ConnectionOptions
    {
        public EndPoint EndPoint { get; set; }

        public TextWriter LogWriter { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        internal MsgPackContext MsgPackContext { get; } = MsgPackContextFactory.Create();

        public int StreamBufferSize { get; set; } = 4096;

        public bool GuestMode { get; set; } = true;
    }
}