using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Eval_Should : TestBase
    {
        [Fact]
        public async Task evaluate_expression()
        {
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Eval<TarantoolTuple<int, int, int>, int>("return ...", TarantoolTuple.Create(1, 2, 3));

                result.Data.ShouldBe(new[] {1, 2, 3});
            }
        }

        [Fact]
        public async Task evaluate_scalar()
        {
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Eval<int>("return 1");

                result.Data.ShouldBe(new [] { 1 });
            }
        }

        [Fact]
        public async Task evaluate_call_function()
        {
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Eval<TarantoolTuple<int, int, int>, TarantoolTuple<int, int>>("return return_tuple()", TarantoolTuple.Create(1, 2, 3));

                result.Data.ShouldBe(new[] { TarantoolTuple.Create(1, 2) });
            }
        }

        [Fact]
        public async Task evaluate_return_null()
        {
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Eval<TarantoolTuple, TarantoolTuple<int, int>>("return return_null()", TarantoolTuple.Empty);

                result.Data.ShouldBe(new[] { default(TarantoolTuple<int, int>) });
            }
        }
    }
}