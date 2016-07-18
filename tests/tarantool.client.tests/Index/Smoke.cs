using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Model.UpdateOperations;

using Tuple = Tarantool.Client.Model.Tuple;

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

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(spaceName);

            var index = await space.GetIndex("primary");

            DataResponse<Model.Tuple<int, string>[]> insertDataResponse;
            try
            {
                insertDataResponse = await index.Insert(Tuple.Create(2, "Music"));
            }
            catch (ArgumentException)
            {
                var deleteResponse = await index.Delete<Model.Tuple<int>, Model.Tuple<int, string, double>>(Tuple.Create(2));
                insertDataResponse = await index.Insert(Tuple.Create(2, "Music"));
            }

            var selectResponse = await index.Select<Model.Tuple<uint>, Model.Tuple<int, string>>(Tuple.Create(1029u));
            var replaceResponse = await index.Replace(Tuple.Create(2, "Car", -245.3));
            var updateResponse = await index.Update<Model.Tuple<int, string, double>, Model.Tuple<int>>(
                Tuple.Create(2),
                new UpdateOperation[] { UpdateOperation.CreateAddition(100, 2) });

            await index.Upsert(Tuple.Create(5u), new UpdateOperation[] { UpdateOperation.CreateAssign(2, 2) });
            await index.Upsert(Tuple.Create(5u), new UpdateOperation[] { UpdateOperation.CreateAddition(-2, 2) });

            var selectResponse2 = index.Select<Model.Tuple<uint>, Model.Tuple<int, int, int>>(Tuple.Create(5u));
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

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(spaceName);

            var index = await space.GetIndex("treeIndex");

            var min2 = await index.Min<Model.Tuple<int, int, int>, Model.Tuple<int>>(Tuple.Create(3));
            var min = await index.Min<Model.Tuple<int, string, double>>();

            var max = await index.Max<Model.Tuple<int, int, int>>();
            var max2 = await index.Max<Model.Tuple<int, string, double>, Model.Tuple<int>>(Tuple.Create(4));
        }
    }
}