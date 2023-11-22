using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests
{
    internal class ConnectionStringFactory
    {
        public static async Task<string> GetReplicationSource_1_7(string userName = null)
        {
            return GetReplicationSource(userName, $"{await ResolveHostname("localhost")}:3301", "TARANTOOL_1_7_REPLICATION_SOURCE");
        }

        public static async Task<string> GetReplicationSource_1_8(string userName = null)
        {
            return GetReplicationSource(userName, $"{await ResolveHostname("localhost")}:3302", "TARANTOOL_1_8_REPLICATION_SOURCE");
        }

        public static async Task<string> GetRedisConnectionString()
        {
            var redisUrl = Environment.GetEnvironmentVariable("REDIS_HOST");

            return $"{await ResolveHostname(redisUrl)}:6379";
        }
        
        public static string GetLatestTarantoolConnectionString(string userName = null, string password = null)
        {
            userName ??= "admin";
            password ??= "adminPassword";
            return BuildConnectionString(userName, password, 3311);
        }

        private static string BuildConnectionString(string userName, string password, int port)
        {
            var userToken = (userName, password)
                switch
                {
                    (null, null) => "",
                    (_, null) => $"{userName}@",
                    _ => $"{userName}:{password}@",
                };
            return $"{userToken}127.0.0.1:{port}";
        }

        private static async Task<string> ResolveHostname(string host)
        {
            if (!string.IsNullOrWhiteSpace(host))
            {
                var resolved = await Dns.GetHostAddressesAsync(host);
                var ip = resolved.First().ToString();
                if (ip == "::1")
                {
                    return "127.0.0.1";
                }
            }

            return "127.0.0.1";
        }

        private static string GetReplicationSource(string userName, string defaultString, string envName)
        {
            userName = string.IsNullOrWhiteSpace(userName) ? string.Empty : userName + "@";

            return $"{userName}{Environment.GetEnvironmentVariable(envName) ?? defaultString}";
        }
    }
}
