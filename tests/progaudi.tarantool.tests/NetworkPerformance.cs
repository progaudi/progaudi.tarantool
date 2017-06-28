using System;
using System.Linq;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using StackExchange.Redis;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class NetworkPerformance : IAsyncLifetime
    {
        Client.Box _tarantoolClient;
        private readonly StringWriterLog _stringWriterLog = new StringWriterLog();
        private ConnectionMultiplexer _multiplexer;

        [Fact]
        public async Task ParallelTest()
        {
            var tasks = new Task[1000];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = _tarantoolClient.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>(
                    "math.sqrt",
                    TarantoolTuple.Create(1.3))
                    .ContinueWith(x =>
                    {
                        var result = x.Result;
                        var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

                        diff.ShouldBeLessThan(double.Epsilon);
                    });
            }

            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task SerialTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var result = await _tarantoolClient.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>(
                    "math.sqrt",
                    TarantoolTuple.Create(1.3));
                var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

                diff.ShouldBeLessThan(double.Epsilon);
            }
        }

        [Fact]
        public async Task ParallelRedisTest()
        {
            var tasks = new Task[1000];
            for (var i = 0; i < tasks.Length; i++)
            {
                tasks[i] = _multiplexer.GetDatabase().ScriptEvaluateAsync("return math.sqrt(9)")
                    .ContinueWith(x =>
                    {
                        var result = (int) x.Result;
                        result.ShouldBe(3);
                    });
            }

            await Task.WhenAll(tasks);
        }

        [Fact]
        public async Task SerialRedisTest()
        {
            for (var i = 0; i < 1000; i++)
            {
                var result = (int)await _multiplexer.GetDatabase().ScriptEvaluateAsync("return math.sqrt(9)");
                result.ShouldBe(3);
            }
        }

        Task IAsyncLifetime.DisposeAsync()
        {
            _tarantoolClient?.Dispose();
            _multiplexer?.Dispose();
            return Task.CompletedTask;
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            var options = new ClientOptions(ConnectionStringFactory.GetReplicationSource(), _stringWriterLog);
            _tarantoolClient = new Client.Box(options);
            await _tarantoolClient.Connect();
            _multiplexer = await ConnectionMultiplexer.ConnectAsync(await ConnectionStringFactory.GetRedisConnectionString());
        }
    }
}
