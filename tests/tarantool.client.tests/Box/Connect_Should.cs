using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Connect_Should
    {
        [Fact]
        public async Task throw_exception_if_UserName_is_null_and_not_GuestMode()
        {
            var options = new ClientOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = null,
                GuestMode = false
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect().ShouldThrowAsync<InvalidOperationException>();
        }

        [Fact]
        public async Task connect_if_UserName_is_null_and_GuestMode()
        {
            var options = new ClientOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = null,
                GuestMode = true
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect();
        }

        [Fact]
        public async Task throw_exception_if_password_is_wrong()
        {
            var options = new ClientOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = "operator",
                Password = "wrongPassword",
                GuestMode = false
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect().ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task throw_exception_if_password_is_empty_for_user_with_unset_password()
        {
            var options = new ClientOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = "notSetPassword",
                Password = string.Empty,
                GuestMode = false
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect().ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task connect_if_password_is_empty_for_user_with_empty_password()
        {
            var options = new ClientOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = "emptyPassword",
                Password = string.Empty
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect();
        }

        [Fact]
        public async Task connect_with_credentials()
        {
            var options = new ClientOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = "operator",
                Password = "operator"

            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();
        }
    }
}
