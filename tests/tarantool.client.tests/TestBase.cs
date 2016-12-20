using System;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class TestBase : IDisposable
    {
        public void Dispose()
        {
           ClearDataAsync().GetAwaiter().GetResult();
        }

        private async Task ClearDataAsync()
        {
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("admin:adminPassword")))
            {
                await tarantoolClient.Call("clear_data");
            }
        }
    }
}