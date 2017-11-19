using System;
using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model.Responses;
using Shouldly;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests.Sql
{
    public class SelectTest : TestBase
    {
        [Fact]
        public async Task Exception_1_7()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_7()))
            {
                box.Info.Version.Major.ShouldBe((1, 7));
                var e = await Should.ThrowAsync<InvalidOperationException>(async () => await box.ExecuteSql("select 1"));
                e.Message.ShouldBe($"Can't use sql on '{box.Info.Version}' of tarantool. Upgrade to 1.8 (prefer latest one).");
            }
        }

        [Fact]
        public async Task SelectData_1_8()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_8()))
            {
                box.Info.Version.Major.ShouldBe((1, 8));
                var result = await box.ExecuteSql<(int, string, int)>("select 1 as ABC, 'z', 3");
                result.Data.ShouldNotBeNull();
                result.Data.ShouldBe(new [] {(1, "z", 3)});

                result.MetaData.ShouldNotBeNull();
                result.MetaData.ShouldBe(new []
                {
                    new FieldMetadata("ABC"),
                    new FieldMetadata("'z'"),
                    new FieldMetadata("3")
                });
            }
        }
    }
}
