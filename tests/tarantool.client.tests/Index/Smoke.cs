using System;
using System.Threading.Tasks;

using Xunit;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.Responses;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Tests.Index
{
    public class Smoke
    {
        [Fact]
        public async Task HashIndexMethods()
        {
            const string spaceName = "primary_only_index";
            var options = new ClientOptions("127.0.0.1:3301");
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(spaceName);

            var index = await space.GetIndex("primary");

            DataResponse<TarantoolTuple<int, string>[]> insertDataResponse;
            try
            {
                insertDataResponse = await index.Insert(TarantoolTuple.Create(2, "Music"));
            }
            catch (ArgumentException)
            {
                var deleteResponse = await index.Delete<TarantoolTuple<int>, TarantoolTuple<int, string, double>>(TarantoolTuple.Create(2));
                insertDataResponse = await index.Insert(TarantoolTuple.Create(2, "Music"));
            }

            var selectResponse = await index.Select<TarantoolTuple<uint>, TarantoolTuple<int, string>>(TarantoolTuple.Create(1029u));
            var replaceResponse = await index.Replace(TarantoolTuple.Create(2, "Car", -245.3));
            var updateResponse = await index.Update<TarantoolTuple<int, string, double>, TarantoolTuple<int>>(
                TarantoolTuple.Create(2),
                new UpdateOperation[] { UpdateOperation.CreateAddition(100, 2) });

            await index.Upsert(TarantoolTuple.Create(5u), new UpdateOperation[] { UpdateOperation.CreateAssign(2, 2) });
            await index.Upsert(TarantoolTuple.Create(5u), new UpdateOperation[] { UpdateOperation.CreateAddition(-2, 2) });

            var selectResponse2 = index.Select<TarantoolTuple<uint>, TarantoolTuple<int, int, int>>(TarantoolTuple.Create(5u));
        }

        [Fact]
        public async Task TreeIndexMethods()
        {
            const string spaceName = "primary_and_secondary_index";
            var options = new ClientOptions("127.0.0.1:3301");
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(spaceName);

            var index = await space.GetIndex("treeIndex");

            var min2 = await index.Min<TarantoolTuple<int, int, int>, TarantoolTuple<int>>(TarantoolTuple.Create(3));
            var min = await index.Min<TarantoolTuple<int, string, double>>();

            var max = await index.Max<TarantoolTuple<int, int, int>>();
            var max2 = await index.Max<TarantoolTuple<int, string, double>, TarantoolTuple<int>>(TarantoolTuple.Create(4));
        }
    }
}