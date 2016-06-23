using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

using Tarantool.Client.Model;

namespace Tarantool.Client.Tests.Space
{
    [TestFixture]
    public class GetIndex_Should
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

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(VSpaceName);

            await space.GetIndex("non-existing").ShouldThrowAsync<ArgumentException>();
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

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(VSpaceName);

            await space.GetIndex(12341234).ShouldThrowAsync<ArgumentException>();
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

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(VSpaceName);

            var index = await space.GetIndex(indexId);

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

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(VSpaceName);

            var index = await space.GetIndex(indexName);

            index.Name.ShouldBe(indexName);
        }
    }
}