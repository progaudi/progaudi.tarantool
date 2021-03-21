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
            const string spaceName = "primary_only_index";
            
            await ClearDataAsync(spaceName);

            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var index = tarantoolClient.GetSchema()[spaceName]["primary"];

                try
                {
                    await index.Insert((2, "Music", 0.0));
                }
                catch (ArgumentException)
                {
                    await index.Delete<ValueTuple<int>, ValueTuple<int, string, double>>(ValueTuple.Create(2));
                    await index.Insert((2, "Music", 0.0));
                }

                await index.Select<ValueTuple<uint>, ValueTuple<int, string>>(ValueTuple.Create(1029u));
                await index.Replace((2, "Car", -245.3));
                await index.Update<ValueTuple<int, string, double>, ValueTuple<int>>(
                    ValueTuple.Create(2),
                    new UpdateOperation[] { UpdateOperation.CreateAddition(100, 2) });

                await index.Upsert((6u, "name", 100.0), new UpdateOperation[] { UpdateOperation.CreateAssign(2, 2) });
                await index.Upsert((6u, "name", 100.0), new UpdateOperation[] { UpdateOperation.CreateAddition(-2, 2) });

                var result = await index.Select<ValueTuple<uint>, ValueTuple<uint, string, double>>(ValueTuple.Create(6u));
                result.Data[0].Item1.ShouldBe(6u);
            }
        }

        [Fact]
        public async Task TreeIndexMethods()
        {
            const string spaceName = "space_TreeIndexMethods";

            await ClearDataAsync(spaceName);

            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = schema[spaceName];

                var index = space["treeIndex"];

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