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

            var selectResponse = await index.Select<Model.Tuple<int>, Model.Tuple<int, string>>(Tuple.Create(2));
            var replaceResponse = await index.Replace(Tuple.Create(2, "Car", -245.3));
            var updateResponse = await index.Update<Model.Tuple<int, string, double>, Model.Tuple<int>, int>(
                Tuple.Create(2),
                UpdateOperation.CreateAddition(100, 2));

            var upsertResponse = await index.Upsert<Model.Tuple<int>, int, Model.Tuple<int, int>>(
                Tuple.Create(5),
                UpdateOperation.CreateAssign(2, 2));
            upsertResponse = await index.Upsert<Model.Tuple<int>, int, Model.Tuple<int, int>>(
                Tuple.Create(5),
                UpdateOperation.CreateAddition(-2, 2));
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

            var min2 = index.Min<Model.Tuple<int, int, int>, Model.Tuple<int>>(Tuple.Create(3));
            var min = index.Min<Model.Tuple<int, string, double>>();

            var max = index.Max<Model.Tuple<int, int, int>>();
            var max2 = index.Max<Model.Tuple<int, string, double>, Model.Tuple<int>>(Tuple.Create(4));
        }
    }
}