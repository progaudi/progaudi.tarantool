using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Data.UpdateOperations;

using Tuple = Tarantool.Client.IProto.Tuple;

namespace Tarantool.Client.Tests.Index
{
    [TestFixture]
    public class Smoke
    {
        [Test]
        public async Task HashIndexMethods()
        {
            const string spaceName = "primary_only_index";
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(spaceName);

            var index = await space.GetIndexAsync("primary");

            ResponsePacket<IProto.Tuple<int, string>[]> insertResponse;
            try
            {
                insertResponse = await index.Insert(Tuple.Create(2, "Music"));
            }
            catch (ArgumentException e)
            {
                var deleteResponse = await index.Delete<IProto.Tuple<int>, IProto.Tuple<int, string, double>>(Tuple.Create(2));
                insertResponse = await index.Insert(Tuple.Create(2, "Music"));
            }

            var selectResponse = await index.Select<IProto.Tuple<int>, IProto.Tuple<int, string>>(Tuple.Create(2));
            var replaceResponse = await index.Replace(Tuple.Create(2, "Car", -245.3));
            var updateResponse = await index.Update<IProto.Tuple<int, string, double>, IProto.Tuple<int>, int>(
                Tuple.Create(2),
                UpdateOperation<int>.CreateAddition(100, 2));

            var upsertResponse = await index.Upsert<IProto.Tuple<int>, int, IProto.Tuple<int, int>>(
                Tuple.Create(5),
                UpdateOperation<int>.CreateAssign(2, 2));
            upsertResponse = await index.Upsert<IProto.Tuple<int>, int, IProto.Tuple<int, int>>(
                Tuple.Create(5),
                UpdateOperation<int>.CreateAddition(-2, 2));
        }

        [Test]
        public async Task TreeIndexMethods()
        {
            const string spaceName = "primary_and_secondary_index";
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(spaceName);

            var index = await space.GetIndexAsync("treeIndex");

            var min2 = index.Min<IProto.Tuple<int, int, int>, IProto.Tuple<int>>(Tuple.Create(3));
            var min = index.Min<IProto.Tuple<int, string, double>>();

            var max = index.Max<IProto.Tuple<int, int, int>>();
            var max2 = index.Max<IProto.Tuple<int, string, double>, IProto.Tuple<int>>(Tuple.Create(4));
        }
    }
}