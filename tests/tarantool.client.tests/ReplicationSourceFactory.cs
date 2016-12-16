using System;

namespace ProGaudi.Tarantool.Client.Tests
{
    internal class ReplicationSourceFactory
    {
        public static string GetReplicationSource(string userName = null)
        {
            var tarantoolUrl = Environment.GetEnvironmentVariable("TARANTOOL_REPLICATION_SOURCE");

            userName = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName + "@";

            return $"{userName}{tarantoolUrl ?? "127.0.0.1:3301"}";
        }
    }
}
