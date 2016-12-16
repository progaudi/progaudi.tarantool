using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class Upsert_Should
    {
        [Fact(Skip = "Bug in tarantool: https://github.com/tarantool/tarantool/issues/1867")]
        public async Task throw_expection_on_space_with_secondary_index()
        {
            const string spaceName = "primary_and_secondary_index";
            using (var tarantoolClient = await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(spaceName);

                await space.Upsert(TarantoolTuple.Create(5), new UpdateOperation[] {UpdateOperation.CreateAddition(1, 2)}).ShouldThrowAsync<ArgumentException>();
            }
        }
    }
}