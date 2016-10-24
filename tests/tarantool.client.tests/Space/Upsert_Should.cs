using System;
using System.Net;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

using Tuple = ProGaudi.Tarantool.Client.Model.Tuple;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class Upsert_Should
    {
        [Fact]
        public async Task throw_expection_on_space_with_secondary_index()
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

            await space.Upsert(Tuple.Create(5), new UpdateOperation[] { UpdateOperation.CreateAddition(1, 2) }).ShouldThrowAsync<ArgumentException>();
        }
    }
}