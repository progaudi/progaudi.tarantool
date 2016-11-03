using System.Collections.Generic;

namespace ProGaudi.Tarantool.Client.Model
{
    public class ConnectionOptions
    {
        public ConnectionOptions()
        {
        }

        public ConnectionOptions(string replicationSource, ILog log)
        {
            if (!string.IsNullOrEmpty(replicationSource))
            {
                Parse(replicationSource, log);
            }
        }

        private void Parse(string replicationSource, ILog log)
        {
            var urls = replicationSource.Split(',');

            foreach (var url in urls)
            {
                var node = TarantoolNode.TryParse(url, log);
                if (node != null)
                {
                    Nodes.Add(node);
                }
            }
        }

        public int ReceiveBufferSize { get; set; } = 8192;
        public int SendBufferSize { get; set; } = 8192;
        public int SendTimeout { get; set; } = -1;
        public int ReceiveTimeout { get; set; } = -1;
        public List<TarantoolNode> Nodes { get; set; } = new List<TarantoolNode>();
    }
}