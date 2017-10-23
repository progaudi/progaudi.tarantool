using System;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

using ProGaudi.Tarantool.Client.Model;

namespace ProGaudi.Tarantool.Client.Tests
{
    public class Performance : TestBase
    {
        [Fact(Skip = "Added just for profiling")]
        public void MultithreadTest()
        {
            var logWriter = new StringWriterLog();
            var threadsCount = 100;
            const string spaceName = "performance";

            using (var tarantoolClient = new Client.Box(new ClientOptions(ConnectionStringFactory.GetReplicationSource(), logWriter)))
            {
                tarantoolClient.Connect().GetAwaiter().GetResult();

                var schema = tarantoolClient.GetSchema();

                var index = schema[spaceName]["primary"];
                var startTime = DateTime.Now;

                logWriter.WriteLine("Before start thread");

                var tasks = new Task[threadsCount];
                for (uint i = 0; i < threadsCount; i++)
                {
                    var client = new TestClient(index, i);
                    tasks[i] = Task.Factory.StartNew(client.Start);
                }

                Task.WaitAll(tasks);

                var endTime = DateTime.Now;

                logWriter.WriteLine($"Time taken:{(endTime - startTime).TotalMilliseconds} ms");
            }
        }
    }

    public class TestClient
    {
        private const uint OperationsCount = 1000;

        private readonly Client.IIndex _index;

        private readonly uint _id;

        public TestClient(Client.IIndex index, uint threadId)
        {
            _index = index;
            _id = threadId;
        }

        public void Start()
        {
            StartImpl().GetAwaiter().GetResult();
        }

        private async Task StartImpl()
        {
            for (uint i = 0; i < OperationsCount; i++)
            {
                var operationId = _id * OperationsCount + i + 1000;

                var existing = await _index.Select<TarantoolTuple<uint>, TarantoolTuple<uint, string>>(TarantoolTuple.Create(operationId));
                if (existing.Data.Any())
                {
                    await _index.Delete<TarantoolTuple<uint>, TarantoolTuple<uint, string>>(TarantoolTuple.Create(operationId));
                }
                await _index.Insert(TarantoolTuple.Create(operationId, $"Insert operation {operationId}"));

                await _index.Select<TarantoolTuple<uint>, TarantoolTuple<uint, string>>(TarantoolTuple.Create(operationId));
            }
        }
    }
}