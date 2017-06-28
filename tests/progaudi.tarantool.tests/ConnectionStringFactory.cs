using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests
{
    internal class ConnectionStringFactory
    {
        public static string GetReplicationSource(string userName = null)
        {
            var tarantoolUrl = Environment.GetEnvironmentVariable("TARANTOOL_REPLICATION_SOURCE");

            userName = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName + "@";

            return $"{userName}{tarantoolUrl ?? "127.0.0.1:3301"}";
        }

        public static async Task<string> GetRedisConnectionString()
        {
            var tarantoolUrl = Environment.GetEnvironmentVariable("TARANTOOL_REPLICATION_SOURCE");

            var host = "127.0.0.1";
            if (tarantoolUrl != null)
            {
                var resolved = await Dns.GetHostAddressesAsync("redis");
                host = resolved.First().ToString();
            }

            return $"{host}:6379";
        }
    }
}
