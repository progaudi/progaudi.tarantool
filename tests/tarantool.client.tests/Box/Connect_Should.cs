using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

using Tarantool.Client.Model;

namespace Tarantool.Client.Tests.Box
{
    [TestFixture]
    public class Connect_Should
    {
        [Test]
        public async Task throw_exception_if_UserName_is_null_and_not_GuestMode()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = null,
                GuestMode = false
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect().ShouldThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task connect_if_UserName_is_null_and_GuestMode()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = null,
                GuestMode = true
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect();
        }

        [Test]
        public async Task throw_exception_if_password_is_wrong()
        {
            var options = new ConnectionOptions()
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

        [Test]
        public async Task throw_exception_if_password_is_empty_for_user_with_unset_password()
        {
            var options = new ConnectionOptions()
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

        [Test]
        public async Task connect_if_password_is_empty_for_user_with_empty_password()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog(),
                UserName = "emptyPassword",
                Password = string.Empty
            };

            var tarantoolClient = new Client.Box(options);


            await tarantoolClient.Connect();
        }

        [Test]
        public async Task connect_with_credentials()
        {
            var options = new ConnectionOptions()
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
