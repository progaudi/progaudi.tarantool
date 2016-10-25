using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Eval_Should
    {
        [Fact]
        public async Task evaluate_expression()
        {
            var tarantoolClient = await Client.Box.Connect("127.0.0.1:3301");

            var result = await tarantoolClient.Eval<TarantoolTuple<int, int, int>, int>("return ...", TarantoolTuple.Create(1, 2, 3));

            result.Data.ShouldBe(new[] {1, 2, 3});
        }
    }
}