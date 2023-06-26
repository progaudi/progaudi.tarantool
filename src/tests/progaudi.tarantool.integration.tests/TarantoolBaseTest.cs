using ProGaudi.Tarantool.Client;

namespace progaudi.tarantool.integration.tests
{
    public class TarantoolBaseTest
    {
        public static async Task<Box> GetTarantoolClient(string userName = null, string password = null)
        {
            userName ??= "admin";
            password ??= "adminPassword";
            return await Box.Connect(BuildConnectionString(userName, password));
        }

        private static string BuildConnectionString(string userName, string password)
        {
            var userToken = (userName, password)
            switch
            {
                (null, null) => "",
                (_, null) => $"{userName}@",
                _ => $"{userName}:{password}@",
            };
            return $"{userToken}{Environment.GetEnvironmentVariable("TARANTOOL_HOST_FOR_TESTS") ?? "127.0.0.1:3310"}";
        }
    }
}
