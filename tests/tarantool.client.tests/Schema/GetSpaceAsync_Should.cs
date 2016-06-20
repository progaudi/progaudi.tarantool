using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

namespace Tarantool.Client.Tests.Schema
{
    [TestFixture]
    public class GetSpaceAsync_Should
    {
        [Test]
        public async Task throw_expection_for_non_existing_space_by_name()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            await schema.GetSpaceAsync("non-existing").ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task throw_expection_for_non_existing_space_by_id()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            await schema.GetSpaceAsync(12341234).ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task returns_space()
        {
            const uint VSpaceId = 0x121; // that space always exist and contains other spaces.
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(VSpaceId);

            space.Id.ShouldBe(VSpaceId);
        }
    }
}