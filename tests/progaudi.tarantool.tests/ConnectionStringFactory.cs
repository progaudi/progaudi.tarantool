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
            return GetReplicationSource(userName, "tarantool_1_7", 3301);
        }

        public static string GetReplicationSource_1_8(string userName = null)
        {
            return GetReplicationSource(userName, "tarantool_1_8", 3302);
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

        private static string GetReplicationSource(string userName, string hostInDocker, int portOnDev)
        {
            var devMachine = string.IsNullOrEmpty(Environment.GetEnvironmentVariable("TARANTOOL_REPLICATION_SOURCE"));

            var host = devMachine ? "localhost" : hostInDocker;
            var port = devMachine ? portOnDev : 3301;

            userName = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName + "@";

            return $"{userName}{host}:{port}";
        }
    }
}
