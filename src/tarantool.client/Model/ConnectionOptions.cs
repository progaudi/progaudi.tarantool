using System.Net;

using MsgPack.Light;

namespace Tarantool.Client.Model
{
    public class ConnectionOptions
    {
        public EndPoint EndPoint { get; set; }

        public ILog LogWriter { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public MsgPackContext MsgPackContext { get; set; } = new MsgPackContext();

        public int StreamBufferSize { get; set; } = 4096;

        public bool GuestMode { get; set; } = true;
    }
}