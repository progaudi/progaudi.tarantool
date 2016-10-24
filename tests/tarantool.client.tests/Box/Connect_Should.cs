using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Connect_Should
    {
        [Fact]
        public async Task connect_if_UserName_is_null_and_GuestMode()
        {
            var options = new ClientOptions("127.0.0.1:3301");

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();
        }

        [Fact]
        public async Task throw_exception_if_password_is_wrong()
        {
            var options = new ClientOptions("operator:wrongPassword@127.0.0.1:3301");

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect().ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task throw_exception_if_password_is_empty_for_user_with_unset_password()
        {
            var options = new ClientOptions("notSetPassword:@127.0.0.1:3301");

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect().ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task connect_if_password_is_empty_for_user_with_empty_password()
        {
            var options = new ClientOptions("emptyPassword:@127.0.0.1:3301");

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();
        }

        [Fact]
        public async Task connect_with_credentials()
        {
            var options = new ClientOptions("operator:operator@127.0.0.1:3301");

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();
        }
    }
}
