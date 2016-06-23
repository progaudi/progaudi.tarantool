using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

using Tarantool.Client.Model;

namespace Tarantool.Client.Tests.Schema
{
    [TestFixture]
    public class GetSpace_Should
    {
        [Test]
        public async Task throw_expection_for_non_existing_space_by_name()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            await schema.GetSpace("non-existing").ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task throw_expection_for_non_existing_space_by_id()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            await schema.GetSpace(12341234).ShouldThrowAsync<ArgumentException>();
        }

        [Test]
        public async Task returns_space_by_id()
        {
            const uint VSpaceId = 0x119; // that space always exist and contains other spaces.
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(VSpaceId);

            space.Id.ShouldBe(VSpaceId);
        }

        [Test]
        public async Task returns_space_by_name()
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

            space.Name.ShouldBe(VSpaceName);
        }

        [Test]
        public async Task read_multiple_spaces_in_a_row()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            for (int i = 0; i < 10; i++)
            {
                var space = await schema.GetSpace(VSpaceName);

                space.Name.ShouldBe(VSpaceName);
            }
        }
    }
}