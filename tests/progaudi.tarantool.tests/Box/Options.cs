using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Options
    {
        [Fact]
        public async Task ReadSchemaOnConnectEnabled()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var utcnow = DateTimeOffset.UtcNow;
                box.Schema.LastReloadTime.ShouldBeInRange(utcnow.AddSeconds(-2), utcnow);
            }
        }

        [Fact]
        public async Task ReadSchemaOnConnectDisabled()
        {
            var options = new ClientOptions(ConnectionStringFactory.GetReplicationSource_1_7());
            options.ConnectionOptions.ReadSchemaOnConnect = false;
            using (var box = new Client.Box(options))
            {
                await box.Connect();
                box.Schema.LastReloadTime.ShouldBe(DateTimeOffset.MinValue);
            }
        }

        [Fact]
        public async Task ReadBoxInfoOnConnectEnabled()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                box.Info.ShouldNotBeNull();
            }
        }

        [Fact]
        public async Task ReadBoxInfoOnConnectDisabled()
        {
            var options = new ClientOptions(ConnectionStringFactory.GetReplicationSource_1_7());
            options.ConnectionOptions.ReadBoxInfoOnConnect = false;
            using (var box = new Client.Box(options))
            {
                await box.Connect();
                box.Info.ShouldBeNull();
            }
        }

        [Fact]
        public async Task ShouldThrowIfThereAreNoNodesWereConfigured()
        {
            var options = new ClientOptions();
            using (var box = new Client.Box(options))
            {
                await Assert.ThrowsAsync<ClientSetupException>(() => box.Connect());
            }
        }
    }
}
