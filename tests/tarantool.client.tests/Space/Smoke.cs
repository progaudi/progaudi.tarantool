using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

using NUnit.Framework;

using Shouldly;

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
                var deleteResponse = await space.Delete<Model.Tuple<uint>, Model.Tuple<uint, string, double>>(Tuple.Create(2u));
                insertDataResponse = await space.Insert(Tuple.Create(2, "Music"));
            }

            var selectResponse = await space.Select<Model.Tuple<uint>, Model.Tuple<uint, string>>(Tuple.Create(2u));
            var replaceResponse = await space.Replace(Tuple.Create(2, "Car", -24.5));
            var updateResponse = await space.Update<Model.Tuple<uint>, Model.Tuple<uint, string, double>>(
                Tuple.Create(2u),
                new UpdateOperation[] { UpdateOperation.CreateAddition(1, 2) });
            await space.Upsert(
                Tuple.Create(5u, 20),
                new UpdateOperation[] { UpdateOperation.CreateAssign(1, 1) });
        }


        [Test]
        public async Task LongTest()
        {

            const string spaceName = "primary_only_index";
            var options = new ConnectionOptions()
            {
                EndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3301),
                LogWriter = new StringWriterLog()
            };

            var tarantoolClient = new Client.Box(options);

            await tarantoolClient.Connect();

            var schema = tarantoolClient.GetSchema();

            var space = await schema.GetSpace(spaceName);

            var longString =
                "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.";
            DataResponse<Model.Tuple<uint, string>[]> insertDataResponse;
            try
            {
                insertDataResponse = await space.Insert(Tuple.Create(222u, longString));
            }
            catch (ArgumentException)
            {
                var deleteResponse = await space.Delete<Model.Tuple<uint>, Model.Tuple<uint, string>>(Tuple.Create(222u));
                insertDataResponse = await space.Insert(Tuple.Create(222u, longString));
            }

            var selectResponse = await space.Select<Model.Tuple<uint>, Model.Tuple<uint, string>>(Tuple.Create(222u));
            selectResponse.Data.Single().Item2.ShouldBe(longString);
        }
    }
}