using NUnit.Framework;
using ProGaudi.Tarantool.Client.Model;
using Shouldly;

namespace progaudi.tarantool.integration.tests.TarantoolBox
{
    [TestFixture]
    internal class InsertTests : TarantoolBaseTest
    {
        static readonly string SpaceName = RandomSpaceName();
        const string IndexName = "primary_idx";

        [SetUp]
        public async Task Setup()
        {
            using var tarantoolClient = await GetTarantoolClient();
            await tarantoolClient.Eval<int>($"local mysp = box.schema.space.create('{SpaceName}'); " +
                $"mysp:format({{{{'id',type = 'scalar'}}, {{'a1',type = 'scalar', is_nullable = true}}, {{'a2',type = 'string', is_nullable = true}}, {{'a3',type = 'string', is_nullable = true}}}}); " +
                $"mysp:create_index('{IndexName}', {{parts = {{'id'}}}}); " +
                $"return 1");
        }

        [TearDown]
        public async Task TearDown()
        {
            using var tarantoolClient = await GetTarantoolClient();
            await tarantoolClient.Eval<int>($"box.space.{SpaceName}:drop(); return 1");
        }

        [TestCase(777)]
        public async Task InsertIntToSpaceAndGetItBack_ShouldBeCorrectAsync(int val)
        {
            using var tarantoolClient = await GetTarantoolClient();
            var schema = tarantoolClient.GetSchema();
            var space = schema[SpaceName];
            space.Name.ShouldBe(SpaceName);
            var id = 1;

            await space.Insert(TarantoolTuple.Create(id, val));
            var tuple = await space.Get<ValueTuple<int>, ValueTuple<int, int>>(ValueTuple.Create(id));

            tuple.Item1.ShouldBe(id);
            tuple.Item2.ShouldBe(val);
        }
    }
}
