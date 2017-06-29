using System;
using System.Linq;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

using ProGaudi.Tarantool.Client.Model;
using ProGaudi.Tarantool.Client.Model.UpdateOperations;

namespace ProGaudi.Tarantool.Client.Tests.Space
{
    public class Smoke : TestBase
    {
        [Fact]
        public async Task Test()
        {
            const string spaceName = "primary_only_index";
            using (var tarantoolClient = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(spaceName);

                try
                {
                    await space.Insert(TarantoolTuple.Create(2u, "Music"));
                }
                catch (ArgumentException)
                {
                    await space.Delete<TarantoolTuple<uint>, TarantoolTuple<uint, string, double>>(TarantoolTuple.Create(2u));
                    await space.Insert(TarantoolTuple.Create(2u, "Music"));
                }

                await space.Select<TarantoolTuple<uint>, TarantoolTuple<uint, string>>(TarantoolTuple.Create(2u));
                await space.Replace(TarantoolTuple.Create(2u, "Car", -24.5));
                await space.Update<TarantoolTuple<uint>, TarantoolTuple<uint, string, double>>(
                    TarantoolTuple.Create(2u),
                    new UpdateOperation[] {UpdateOperation.CreateAddition(1, 2)});
                await space.Upsert(
                    TarantoolTuple.Create(5u, 20),
                    new UpdateOperation[] {UpdateOperation.CreateAssign(1, 1)});
            }
        }


        [Fact]
        public async Task LongTest()
        {
            const string spaceName = "primary_only_index";

            using (var tarantoolClient = new Client.Box(new ClientOptions(ConnectionStringFactory.GetReplicationSource(), new StringWriterLog())))
            {
                await tarantoolClient.Connect();

                var schema = tarantoolClient.GetSchema();

                var space = await schema.GetSpace(spaceName);

                var longString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.";
                try
                {
                    await space.Insert(TarantoolTuple.Create(222u, longString));
                }
                catch (ArgumentException)
                {
                    await space.Delete<TarantoolTuple<uint>, TarantoolTuple<uint, string>>(TarantoolTuple.Create(222u));
                    await space.Insert(TarantoolTuple.Create(222u, longString));
                }

                var selectResponse = await space.Select<TarantoolTuple<uint>, TarantoolTuple<uint, string>>(TarantoolTuple.Create(222u));
                selectResponse.Data.Single().Item2.ShouldBe(longString);
            }
        }
    }
}