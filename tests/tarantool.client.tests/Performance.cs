using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Tarantool.Client.Model;

using Tuple = Tarantool.Client.Model.Tuple;

namespace Tarantool.Client.Tests
{
    [TestFixture]
    public class Performance
    {
        [Test]
        [Ignore("Added just for profiling")]
        public void MultithreadTest()
        {
            var logWriter = new StringWriterLog();
            var threadsCount = 100;
            const string spaceName = "performance";

            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = logWriter
            };
            var tarantoolClient = new Tarantool.Client.Box(options);

            tarantoolClient.Connect().GetAwaiter().GetResult();

            var schema = tarantoolClient.GetSchema();

            var space = schema.GetSpace(spaceName).GetAwaiter().GetResult();

            var index = space.GetIndex("primary").GetAwaiter().GetResult();
            var startTime = DateTime.Now;

            logWriter?.WriteLine("Before start thread");

            var tasks = new Task[threadsCount];
            for (uint i = 0; i < threadsCount; i++)
            {
                var client = new TestClient(index, i);
                tasks[i] = Task.Factory.StartNew(client.Start);
            }

            Task.WaitAll(tasks);

            var endTime = DateTime.Now;

            logWriter?.WriteLine($"Time taken:{(endTime - startTime).TotalMilliseconds} ms");
        }           
    }

    public class TestClient
    {
        private const uint OperationsCount = 1000;

        private readonly Client.Index _index;

        private readonly uint _id;

        public TestClient(Client.Index index, uint threadId)
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

                var existing = await _index.Select<Tarantool.Client.Model.Tuple<uint>, Tarantool.Client.Model.Tuple<uint, string>>(Tuple.Create(operationId));
                if (existing.Data.Any())
                {
                    await _index.Delete<Tarantool.Client.Model.Tuple<uint>, Tarantool.Client.Model.Tuple<uint, string>>(Tuple.Create(operationId));
                }
                await _index.Insert(Tuple.Create(operationId, $"Insert operation {operationId}"));

                await _index.Select<Tarantool.Client.Model.Tuple<uint>, Tarantool.Client.Model.Tuple<uint, string>>(Tuple.Create(operationId));
            }
        }
    }
}