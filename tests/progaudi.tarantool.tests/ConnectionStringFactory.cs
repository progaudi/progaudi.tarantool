using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests
{
    internal class ConnectionStringFactory
    {
        public static string GetReplicationSource_1_7(string userName = null)
        {
            return GetReplicationSource(userName, "localhost:3301", "TARANTOOL_1_7_REPLICATION_SOURCE");
        }

        public static string GetReplicationSource_1_8(string userName = null)
        {
            return GetReplicationSource(userName, "localhost:3302", "TARANTOOL_1_8_REPLICATION_SOURCE");
        }

        public static async Task<string> GetRedisConnectionString()
        {
            var tarantoolUrl = Environment.GetEnvironmentVariable("TARANTOOL_1_7_REPLICATION_SOURCE");

            var host = "127.0.0.1";
            if (tarantoolUrl != null)
            {
                var resolved = await Dns.GetHostAddressesAsync("redis");
                host = resolved.First().ToString();
            }

            return $"{host}:6379";
        }

        private static string GetReplicationSource(string userName, string defaultString, string envName)
        {
            userName = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName + "@";

            return $"{userName}{Environment.GetEnvironmentVariable(envName) ?? defaultString}";
        }
    }
}
