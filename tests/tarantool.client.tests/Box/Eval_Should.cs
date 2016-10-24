using System;
using System.Linq;
using System.Net;
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
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var result = await tarantoolClient.Eval<TarantoolTuple<int, int, int>, int>("return ...", TarantoolTuple.Create(1, 2, 3));

            result.Data.ShouldBe(new[] {1, 2, 3});
        }
    }
}