using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Call_Should
    {
        [Fact]
        public async Task call_method()
        {
            using (var tarantoolClient = await Client.Box.Connect("127.0.0.1:3301"))
            {
                var result = await tarantoolClient.Call_16<TarantoolTuple<double>, TarantoolTuple<double>>("math.sqrt", TarantoolTuple.Create(1.3));

                var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

                diff.ShouldBeLessThan(double.Epsilon);
            }
        }

        [Fact]
        public async Task return_null_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect("127.0.0.1:3301"))
            {
                var result = await tarantoolClient.Call_16<TarantoolTuple<string, int>>("return_null");
                result.Data.ShouldBe(new TarantoolTuple<string, int>[] {null});
            }
        }

        [Fact]
        public async Task return_tuple_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect("127.0.0.1:3301"))
            {
                var result = await tarantoolClient.Call_16<TarantoolTuple<int, int>>("return_tuple");
                result.Data.ShouldBe(new[] {TarantoolTuple.Create(1, 2)});
            }
        }

        [Fact]
        public async Task return_int_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect("127.0.0.1:3301"))
            {
                var result = await tarantoolClient.Call_16<TarantoolTuple<int>>("return_scalar");
                result.Data.ShouldBe(new[] { TarantoolTuple.Create(1) });
            }
        }
    }
}