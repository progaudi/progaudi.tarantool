using System;
using System.Threading.Tasks;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class TestBase
    {
        public async Task ClearDataAsync()
        {
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("admin:adminPassword")))
            {
                await tarantoolClient.Call("clear_data");
            }
        }
    }
}