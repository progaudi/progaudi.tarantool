using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class GetIndex_Should
    {
        [Fact]
        public async Task throw_expection_for_non_existing_space_by_name()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(VSpaceName);

                await space.GetIndex("non-existing").ShouldThrowAsync<ArgumentException>();
            }
        }

        [Fact]
        public async Task throw_expection_for_non_existing_space_by_id()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(VSpaceName);

                await space.GetIndex(12341234).ShouldThrowAsync<ArgumentException>();
            }
        }

        [Fact]
        public async Task returns_space_by_id()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            const uint indexId = 2;
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(VSpaceName);

                var index = await space.GetIndex(indexId);

                index.Id.ShouldBe(indexId);
            }
        }

        [Fact]
        public async Task returns_space_by_name()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            const string indexName = "owner";
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(VSpaceName);

                var index = await space.GetIndex(indexName);

                index.Name.ShouldBe(indexName);
            }
        }
    }
}