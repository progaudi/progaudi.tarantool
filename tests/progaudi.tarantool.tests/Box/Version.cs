using System.Threading.Tasks;
using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using Xunit;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Version : TestBase
    {
        [Fact]
        public async Task Smoke()
        {
            var options = new ClientOptions(await ConnectionStringFactory.GetReplicationSource_1_7());
            options.ConnectionOptions.ReadBoxInfoOnConnect = false;
            using (var box = new Client.Box(options))
            {
                await box.Connect();

                await box.ReloadBoxInfo();

                box.Info.ReadOnly.ShouldBeFalse();
            }
        }
    }
}
