using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Connect_Should : TestBase
    {
        [Fact]
        public async Task connect_if_UserName_is_null_and_GuestMode()
        {
            using (await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource()))
            { }
        }

        [Fact]
        public async Task throw_exception_if_password_is_wrong()
        {
            await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("operator:wrongPassword")).ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task throw_exception_if_password_is_empty_for_user_with_unset_password()
        {
            await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("notSetPassword:")).ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task connect_if_password_is_empty_for_user_with_empty_password()
        {
            using (await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("emptyPassword:")))
            { }
        }

        [Fact]
        public async Task connect_with_credentials()
        {
            using (await Client.Box.Connect(ReplicationSourceFactory.GetReplicationSource("operator:operator")))
            { }
        }
    }
}
