using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Call_Should : TestBase
    {
        [Fact]
        public async Task call_method()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>("math.sqrt", TarantoolTuple.Create(1.3));

                var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

                diff.ShouldBeLessThan(double.Epsilon);
            }
        }

        [Fact]
        public async Task return_null_v1_6_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                await Should.ThrowAsync<ArgumentException>(async () => await tarantoolClient.Call_1_6<TarantoolTuple<string, int>>("return_null"));
            }
        }

        [Fact]
        public async Task return_tuple_v1_6_with_null_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call_1_6<TarantoolTuple<string>>("return_tuple_with_null");
                result.Data.ShouldBe(new[] { TarantoolTuple.Create(default(string)) });
            }
        }

        [Fact]
        public async Task return_tuple_v1_6_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call_1_6<TarantoolTuple<int, int>>("return_tuple");
                result.Data.ShouldBe(new[] {TarantoolTuple.Create(1, 2)});
            }
        }

        [Fact]
        public async Task return_int_v1_6_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call_1_6<TarantoolTuple<int>>("return_scalar");
                result.Data.ShouldBe(new[] { TarantoolTuple.Create(1) });
            }
        }

        [Fact]
        public async Task return_nothing_v1_6_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                Should.NotThrow(async () => await tarantoolClient.Call_1_6("return_nothing"));
            }
        }

        [Fact]
        public async Task return_null_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call<TarantoolTuple<string, int>>("return_null");
                result.Data.ShouldBe(new[] { default(TarantoolTuple<string, int>) });
            }
        }

        [Fact]
        public async Task return_tuple_with_null_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call<TarantoolTuple<string>>("return_tuple_with_null");
                result.Data.ShouldBe(new[] { TarantoolTuple.Create(default(string)) });
            }
        }

        [Fact]
        public async Task return_tuple_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call<TarantoolTuple<int, int>>("return_tuple");
                result.Data.ShouldBe(new[] { TarantoolTuple.Create(1, 2) });
            }
        }

        [Fact]
        public async Task return_array_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call<TarantoolTuple<string[]>>("return_array");
                result.Data[0].Item1.ShouldBe(new[] {"abc", "def"});
            }
        }

        [Fact]
        public async Task return_int_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var result = await tarantoolClient.Call<int>("return_scalar");
                result.Data.ShouldBe(new[] { 1 });
            }
        }

        [Fact]
        public async Task return_nothing_should_not_throw()
        {
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                Should.NotThrow(async () => await tarantoolClient.Call("return_nothing"));
            }
        }
    }
}