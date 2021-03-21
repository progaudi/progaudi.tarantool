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
            using (var tarantoolClient = await Client.Box.Connect(await ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                var schema = tarantoolClient.GetSchema();

                var space = schema[spaceName];

                try
                {
                    await space.Insert((2u, "Music", 0.0f));
                }
                catch (ArgumentException)
                {
                    await space.Delete<ValueTuple<uint>, ValueTuple<uint, string, double>>(ValueTuple.Create(2u));
                    await space.Insert((2u, "Music", 0.0f));
                }

                await space.Select<ValueTuple<uint>, ValueTuple<uint, string, double>>(ValueTuple.Create(2u));
                await space.Replace((2u, "Car", -24.5));
                await space.Update<ValueTuple<uint>, ValueTuple<uint, string, double>>(
                    ValueTuple.Create(2u),
                    new UpdateOperation[] {UpdateOperation.CreateAddition(1, 2)});
                await space.Upsert(
                    (5u, "Test", 20),
                    new UpdateOperation[] {UpdateOperation.CreateAssign(1, 1)});
            }
        }

        [Fact]
        public async Task LongTest()
        {
            const string spaceName = "primary_only_index";

            using (var tarantoolClient = new Client.Box(new ClientOptions(await ConnectionStringFactory.GetReplicationSource_1_7(), new StringWriterLog())))
            {
                await tarantoolClient.Connect();

                var schema = tarantoolClient.GetSchema();

                var space = schema[spaceName];

                var longString = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec condimentum pharetra porta. Aliquam ullamcorper ex quis mi aliquet egestas. Suspendisse dui nunc, tincidunt mattis purus ac, fringilla porttitor turpis. Nunc nunc turpis, accumsan eu mi et, scelerisque ultrices orci. Maecenas sed ornare risus. Nam ut luctus ante, id tincidunt diam. Vestibulum maximus non quam molestie rutrum. Phasellus faucibus nunc eu sapien posuere, sed imperdiet quam sollicitudin. Donec nec dui ullamcorper, tincidunt sem eget, egestas lorem. Proin egestas, sem a malesuada sodales, risus sapien aliquet leo, non commodo sem ante et tellus. Donec vel ex et elit pellentesque iaculis faucibus vel lacus. Cum sociis natoque penatibus et magnis dis parturient montes, nascetur ridiculus mus. Etiam fermentum dui at ligula luctus faucibus. Nunc congue placerat dignissim. Quisque odio ipsum, viverra id faucibus sed, pellentesque id velit. Etiam a nibh non lorem vulputate volutpat.";
                try
                {
                    await space.Insert((222u, longString, 0.0f));
                }
                catch (ArgumentException)
                {
                    await space.Delete<ValueTuple<uint>, ValueTuple<uint, string, double>>(ValueTuple.Create(222u));
                    await space.Insert((222u, longString, 0.0f));
                }

                var selectResponse = await space.Select<ValueTuple<uint>, ValueTuple<uint, string, double>>(ValueTuple.Create(222u));
                selectResponse.Data.Single().Item2.ShouldBe(longString);
            }
        }
    }
}