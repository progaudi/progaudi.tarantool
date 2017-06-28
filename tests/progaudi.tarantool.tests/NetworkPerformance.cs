using System;
using System.Text;

using ProGaudi.MsgPack.Light;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model.Requests;
using ProGaudi.Tarantool.Client.Model.Responses;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Tests;
using ProGaudi.Tarantool.Client.Model;
using System.Linq;

public class NetworkPerformance : IAsyncLifetime
{
    Box tarantoolClient;
    [Fact]
    public async Task Test()
    {
        for (int i = 0; i < 1000; i++)
        {
            var result = await tarantoolClient.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>(
                "math.sqrt",
                TarantoolTuple.Create(1.3));
            var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

            diff.ShouldBeLessThan(double.Epsilon);
        }
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        tarantoolClient?.Dispose();
        return Task.CompletedTask;
    }

    async Task IAsyncLifetime.InitializeAsync()
    {
        tarantoolClient = await ProGaudi.Tarantool.Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource());
    }
}
