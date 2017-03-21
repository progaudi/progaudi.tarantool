using System;
using System.Threading.Tasks;

using Xunit;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;
using Shouldly;

namespace ProGaudi.Tarantool.Client.Tests.Index
{
    public class Smoke : TestBase
    {
        [Fact]
        public async Task HashIndexMethods()
        {
            await ClearDataAsync("primary_only_index");

            const string spaceName = "primary_only_index";
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(spaceName);

                var index = await space.GetIndex("primary");

                try
                {
                    await index.Insert(TarantoolTuple.Create(2, "Music"));
                }
                catch (ArgumentException)
                {
                    await index.Delete<TarantoolTuple<int>, TarantoolTuple<int, string, double>>(TarantoolTuple.Create(2));
                    await index.Insert(TarantoolTuple.Create(2, "Music"));
                }

                await index.Select<TarantoolTuple<uint>, TarantoolTuple<int, string>>(TarantoolTuple.Create(1029u));
                await index.Replace(TarantoolTuple.Create(2, "Car", -245.3));
                await index.Update<TarantoolTuple<int, string, double>, TarantoolTuple<int>>(
                    TarantoolTuple.Create(2),
                    new UpdateOperation[] { UpdateOperation.CreateAddition(100, 2) });

                await index.Upsert(TarantoolTuple.Create(6u), new UpdateOperation[] { UpdateOperation.CreateAssign(2, 2) });
                await index.Upsert(TarantoolTuple.Create(6u), new UpdateOperation[] { UpdateOperation.CreateAddition(-2, 2) });

                var result = await index.Select<TarantoolTuple<uint>, TarantoolTuple<uint>>(TarantoolTuple.Create(6u));
                result.Data[0].Item1.ShouldBe(6u);
            }
        }

        [Fact]
        public async Task TreeIndexMethods()
        {
            await ClearDataAsync("space_TreeIndexMethods");

            const string spaceName = "space_TreeIndexMethods";
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(spaceName);

                var index = await space.GetIndex("treeIndex");

                var min2 = await index.Min<TarantoolTuple<int, int, int>, TarantoolTuple<int>>(TarantoolTuple.Create(3));
                min2.ShouldBe(null);
                var min = await index.Min<TarantoolTuple<int, string, double>>();
                min.ShouldBe(null);

                var max = await index.Max<TarantoolTuple<int, int, int>>();
                max.ShouldBe(min2);
                var max2 = await index.Max<TarantoolTuple<int, string, double>, TarantoolTuple<int>>(TarantoolTuple.Create(1));
                max2.ShouldBe(min);
            }
        }
    }
}