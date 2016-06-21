using System;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Tarantool.Client.IProto.Data.Packets;
using Tarantool.Client.IProto.Data.UpdateOperations;

using Tuple = Tarantool.Client.IProto.Tuple;

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

            await tarantoolClient.ConnectAsync();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpaceAsync(spaceName);

            ResponsePacket<IProto.Tuple<int, string>[]> insertResponse;
            try
            {
                insertResponse = await space.Insert(Tuple.Create(2, "Music"));
            }
            catch (ArgumentException e)
            {
                var deleteResponse = await space.Delete<IProto.Tuple<int>, IProto.Tuple<int, string, double>>(Tuple.Create(2));
                insertResponse = await space.Insert(Tuple.Create(2, "Music"));
            }
            
            var selectResponse = await space.Select<IProto.Tuple<int>, IProto.Tuple<int, string>>(Tuple.Create(2));
            var replaceResponse = await space.Replace(Tuple.Create(2, "Car", -24.5));
            var updateResponse = await space.Update<IProto.Tuple<int>, int, IProto.Tuple<int, string, double>> (Tuple.Create(2), UpdateOperation<int>.CreateAddition(1, 2));
            var upsertResponse = await space.Upsert(Tuple.Create(5), UpdateOperation<int>.CreateAddition(1, 2));
        }
    }
}