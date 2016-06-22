using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Tarantool.Client.Model;
using Tarantool.Client.Model.Responses;
using Tarantool.Client.Model.UpdateOperations;

using Tuple = Tarantool.Client.Model.Tuple;

namespace Tarantool.Client.Tests.Space
{
    [TestFixture]
    public class Smoke
    {
        [Test]
        public async Task Test()
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

            DataResponse<Model.Tuple<int, string>[]> insertDataResponse;
            try
            {
                insertDataResponse = await space.Insert(Tuple.Create(2, "Music"));
            }
            catch (ArgumentException)
            {
                var deleteResponse = await space.Delete<Model.Tuple<int>, Model.Tuple<int, string, double>>(Tuple.Create(2));
                insertDataResponse = await space.Insert(Tuple.Create(2, "Music"));
            }
            
            var selectResponse = await space.Select<Model.Tuple<int>, Model.Tuple<int, string>>(Tuple.Create(2));
            var replaceResponse = await space.Replace(Tuple.Create(2, "Car", -24.5));
            var updateResponse = await space.Update<Model.Tuple<int>, int, Model.Tuple<int, string, double>> (Tuple.Create(2), UpdateOperation.CreateAddition(1, 2));
            var upsertResponse = await space.Upsert(Tuple.Create(5), UpdateOperation.CreateAddition(1, 2));
        }
    }
}