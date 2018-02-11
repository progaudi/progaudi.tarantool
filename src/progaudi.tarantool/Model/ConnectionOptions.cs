using System.Collections.Generic;

namespace ProGaudi.Tarantool.Client.Model
{
    using System;

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

        public int ReadStreamBufferSize { get; set; } = 4096;

        public int WriteNetworkTimeout { get; set; } = -1;

        public int ReadNetworkTimeout { get; set; } = -1;

        public int PingCheckInterval { get; set; } = -1;

        public TimeSpan? PingCheckTimeout { get; set; }

        public List<TarantoolNode> Nodes { get; set; } = new List<TarantoolNode>();

        public bool ReadSchemaOnConnect { get; set; } = true;

        public bool ReadBoxInfoOnConnect { get; set; } = true;
    }
}