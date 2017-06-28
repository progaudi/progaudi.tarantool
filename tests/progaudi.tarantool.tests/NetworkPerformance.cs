using System;
using System.Linq;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class NetworkPerformance : IAsyncLifetime
    {
        Client.Box _tarantoolClient;

        [Fact]
        public async Task Test()
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

        Task IAsyncLifetime.DisposeAsync()
        {
            _tarantoolClient?.Dispose();
            return Task.CompletedTask;
        }

        async Task IAsyncLifetime.InitializeAsync()
        {
            _tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource());
        }
    }
}
