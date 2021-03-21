using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class GetIndex_Should : TestBase
    {
        [Fact]
        public async Task throw_expection_for_non_existing_space_by_name()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = schema[VSpaceName];

                Should.Throw<ArgumentException>(() =>
                {
                    var _ = space["non-existing"];
                });
            }
        }

        [Fact]
        public async Task throw_expection_for_non_existing_space_by_id()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = schema[VSpaceName];

                Should.Throw<ArgumentException>(() =>
                {
                    var _ = space[12341234];
                });
            }
        }

        [Fact]
        public async Task returns_space_by_id()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            const uint indexId = 2;
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = schema[VSpaceName];

                var index = space[indexId];

                index.Id.ShouldBe(indexId);
            }
        }

        [Fact]
        public async Task returns_space_by_name()
        {
            const string VSpaceName = "_vspace"; // that space always exist and contains other spaces.
            const string indexName = "owner";
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = schema[VSpaceName];

                var index = space[indexName];

                index.Name.ShouldBe(indexName);
            }
        }
    }
}