using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class TestBase
    {
        public async Task ClearDataAsync(string[] spaceNames)
        {
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("admin:adminPassword")))
            {
                await tarantoolClient.Call("clear_data", TarantoolTuple.Create(spaceNames));
            }
        }
    }
}