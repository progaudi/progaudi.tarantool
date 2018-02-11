using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests.Sql
{
    public class UpdateTest
    {
        [Fact]
        [Trait("Tarantool", "1.8")]
        public async Task Simple()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource_1_8()))
            {
                try
                {
                    await box.Call("create_sql_test");
                    var result = await box.ExecuteSql("update sql_test set name = '1234' where id = 2");
                    result.SqlInfo.ShouldNotBeNull();
                    result.SqlInfo.RowCount.ShouldBe(1);
                }
                finally
                {
                    await box.Call("drop_sql_test");
                }
            }
        }
    }
}
