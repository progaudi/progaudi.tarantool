using System.Collections.Generic;
using ProGaudi.MsgPack.Light;

namespace ProGaudi.Tarantool.Client.Model
{
    public class ConnectionOptions
    {
        public ConnectionOptions()
        {
        }

        public ConnectionOptions(string replicationSource)
        {
        }

        public ILog LogWriter { get; set; }

        public MsgPackContext MsgPackContext { get; set; } = new MsgPackContext();

        public int ReadStreamBufferSize { get; set; } = 4096;

        public int WriteNetworkTimeout { get; set; } = -1;

        public int ReadNetworkTimeout { get; set; } = -1;

        public List<NodeOptions> NodeOptions { get; set; } = new List<NodeOptions>();
    }
}