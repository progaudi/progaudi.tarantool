using NUnit.Framework;
using Shouldly;

namespace progaudi.tarantool.integration.tests.TarantoolBox
{
    [TestFixture]
    public class SchemaTests : TarantoolBaseTest
    {
        const string SpaceName = "schema_test_space";
        const string IndexName = "primary_idx";

        [SetUp]
        public async Task Setup()
        {
            using var tarantoolClient = await GetTarantoolClient();
            await tarantoolClient.Eval<int>($"local mysp = box.schema.space.create('{SpaceName}'); " +
                $"mysp:format({{{{'a',type = 'number'}}}}); " +
                $"mysp:create_index('{IndexName}', {{parts = {{1, 'number'}}}}); " +
                $"return 1");
        }

        [TearDown]
        public async Task TearDown()
        {
            using var tarantoolClient = await GetTarantoolClient();
            await tarantoolClient.Eval<int>($"box.space.{SpaceName}:drop(); return 1");
        }

        [Test]
        public async Task GetNotExistingSpace_ShouldThrowArgumentException()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var schema = tarantoolClient.GetSchema();

            Should.Throw<ArgumentException>(() =>
            {
                var _ = schema["not_existing_space"];
            });
        }

        [Test]
        public async Task GetExistingSpace_ShouldReturnCorrectly()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var schema = tarantoolClient.GetSchema();
            var space = schema[SpaceName];
            space.Name.ShouldBe(SpaceName);
        }

        [Test]
        public async Task GetNotExistingIndex_ShouldThrowArgumentException()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var schema = tarantoolClient.GetSchema();
            var space = schema[SpaceName];

            Should.Throw<ArgumentException>(() =>
            {
                var _ = schema["not_existing_index"];
            });
        }

        [Test]
        public async Task GetExistingIndex_ShouldReturnCorrectly()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var schema = tarantoolClient.GetSchema();
            var space = schema[SpaceName];
            var index = space[IndexName];
            index.Name.ShouldBe(IndexName);
        }
    }
}
