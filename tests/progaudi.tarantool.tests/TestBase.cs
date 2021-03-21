using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class TestBase
    {
        public async Task ClearDataAsync(params string[] spaceNames)
        {
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7("admin:adminPassword")))
            {
                await tarantoolClient.Call("clear_data", TarantoolTuple.Create(spaceNames));
            }
        }
    }
}