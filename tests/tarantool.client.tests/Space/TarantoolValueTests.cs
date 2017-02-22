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
            await ClearDataAsync();

            const string spaceName = "with_scalar_index";
            var clientOptions = new ClientOptions(ReplicationSourceFactory.GetReplicationSource());
            using (var tarantoolClient = new Tarantool.Client.Box(clientOptions))
            {
                await tarantoolClient.Connect();
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
                data[0].Item1.Unpack<bool>(clientOptions.MsgPackContext).ShouldBe(true);
                data[0].Item2.Unpack<string>(clientOptions.MsgPackContext).ShouldBe("Music");

                data[1].Item1.Unpack<ulong>(clientOptions.MsgPackContext).ShouldBe(1u);
                data[1].Item2.Unpack<double>(clientOptions.MsgPackContext).ShouldBe(2f);

                data[2].Item1.Unpack<double>(clientOptions.MsgPackContext).ShouldBe(2f);
                data[2].Item2.Unpack<int[]>(clientOptions.MsgPackContext).ShouldBe(new[] { 1, 2, 3 });

                data[3].Item1.Unpack<string>(clientOptions.MsgPackContext).ShouldBe("false");
                data[3].Item2.Unpack<int[]>(clientOptions.MsgPackContext).ShouldBe(null);
            }
        }
    }
}