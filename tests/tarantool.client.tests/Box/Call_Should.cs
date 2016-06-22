using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

using Tarantool.Client.Model;

using Tuple = Tarantool.Client.Model.Tuple;

namespace Tarantool.Client.Tests.Box
{
    [TestFixture]
    public class Call_Should
    {
        [Test]
        public async Task call_method()
        {
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
            };
            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var result = await tarantoolClient.Call<Model.Tuple<double>, Model.Tuple<double>>("math.sqrt", Tuple.Create(1.3));

            var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

            diff.ShouldBeLessThan(double.Epsilon);
        }
    }
}