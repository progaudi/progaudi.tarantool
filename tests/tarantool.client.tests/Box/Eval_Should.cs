using System.Linq;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

using Tarantool.Client.Model;

using Tuple = Tarantool.Client.Model.Tuple;
using System;

namespace Tarantool.Client.Tests.Box
{
    [TestFixture]
    public class Eval_Should
    {
        [Test]
        public async Task evaluate_expression()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var result = await tarantoolClient.Eval<Model.Tuple<int, int, int>, int>("return ...", Tuple.Create(1, 2, 3));

            result.Data.ShouldBe(new[] {1, 2, 3});
        }
    }
}