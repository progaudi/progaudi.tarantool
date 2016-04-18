using tarantool_client;

using Xunit;

namespace TarantoolDnx.MsgPack.Tests
{
    public class AsyncTarantoolClientTest
    {
        private const int Port = 3301;
        private const string IpAddress = "192.168.99.100";
        private const string UserName = "user";
        private const string Password = "password";

        [Fact(Skip = "Manual run only")]
        public void TestGreetings()
        {
            var asyncClient = new AsyncTarantoolClient();
            asyncClient.Connect(IpAddress, Port);
            asyncClient.Login(UserName, Password);
        }
    }
}