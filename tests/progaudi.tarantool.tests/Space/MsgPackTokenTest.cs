// <copyright file="MsgPackTokenTest.cs" company="eVote">
//   Copyright © eVote
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using ProGaudi.MsgPack.Light;
using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class MsgPackTokenTest
        : TestBase
    {
        [Fact]
        public async Task Smoke()
        {
            await ClearDataAsync(new[] { "with_scalar_index" });

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
                var result = await index.Select<TarantoolTuple<bool>, TarantoolTuple<MsgPackToken, MsgPackToken>>(
                    TarantoolTuple.Create(false),
                    new SelectOptions
                    {
                        Iterator = Model.Enums.Iterator.All
                    });

                result.Data.ShouldNotBeNull();

                var data = result.Data;
                ((bool)data[0].Item1).ShouldBe(true);
                ((string)data[0].Item2).ShouldBe("Music");

                ((ulong)data[1].Item1).ShouldBe(1u);
                ((double)data[1].Item2).ShouldBe(2f);

                ((double)data[2].Item1).ShouldBe(2f);
                ((MsgPackToken[])data[2].Item2).Select(t => (int)t).ToArray().ShouldBe(new[] { 1, 2, 3 });

                ((string)data[3].Item1).ShouldBe("false");
                ((MsgPackToken[])data[3].Item2).ShouldBe(null);
            }
        }
    }
}