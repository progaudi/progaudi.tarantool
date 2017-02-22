using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class TarantoolValueTests : TestBase
    {
        [Fact]
        public async Task Smoke()
        {
            const string spaceName = "with_scalar_index";
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(spaceName);

                await space.Insert(TarantoolTuple.Create(2f, new[] { 1, 2, 3 }));
                await space.Insert(TarantoolTuple.Create(true, "Music"));
                await space.Insert(TarantoolTuple.Create(1u, 2f));
                await space.Insert(TarantoolTuple.Create("false", (string)null));

                var index = await space.GetIndex(0);
                var result = await index.Select<TarantoolTuple<uint>,TarantoolTuple<TarantoolValue, TarantoolValue>>(
                     TarantoolTuple.Create(2u),
                      new SelectOptions()
                      {
                          Iterator = Model.Enums.Iterator.All
                      });

                result.Data.ShouldNotBeNull();

                var data = result.Data;

                data[0].Item1.Unpack<bool>().ShouldBe(true);
                data[0].Item2.Unpack<string>().ShouldBe("Music");

                data[1].Item1.Unpack<ulong>().ShouldBe(1u);
                data[1].Item2.Unpack<double>().ShouldBe(2f);

                data[2].Item1.Unpack<double>().ShouldBe(2f);
                data[2].Item2.Unpack<int[]>().ShouldBe(new[] { 1, 2, 3 });

                data[3].Item1.Unpack<string>().ShouldBe("false");
                data[3].Item2.Unpack<int[]>().ShouldBe(null);
            }
        }
    }
}