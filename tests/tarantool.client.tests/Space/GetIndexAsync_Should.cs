using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

namespace Tarantool.Client.Tests.Space
{
    [TestFixture]
    public class GetIndexAsync_Should
    {
        [Test]
        public async Task throw_expection_for_non_existing_space_by_name()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(VSpaceName);

            await space.GetIndexAsync("non-existing").ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task throw_expection_for_non_existing_space_by_id()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(VSpaceName);

            await space.GetIndexAsync(12341234).ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task returns_space_by_id()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            const uint indexId = 2;
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(VSpaceName);

            var index = await space.GetIndexAsync(indexId);

            index.Id.ShouldBe(indexId);
        }

        [Test]
        public async Task returns_space_by_name()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            const string indexName = "owner";
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(VSpaceName);

            var index = await space.GetIndexAsync(indexName);

            index.Name.ShouldBe(indexName);
        }
    }
}