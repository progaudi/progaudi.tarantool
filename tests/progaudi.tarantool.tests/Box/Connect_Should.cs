using System;
using System.Threading.Tasks;

using Xunit;

using Shouldly;
using ProGaudi.Tarantool.Client.Model;
using System.Linq;

namespace ProGaudi.Tarantool.Client.Tests.Box
{
    public class Connect_Should : TestBase
    {
        [Fact]
        public async Task connect_if_UserName_is_null_and_GuestMode()
        {
            using (await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource()))
            { }
        }

        [Fact]
        public async Task throw_exception_if_password_is_wrong()
        {
            await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource("operator:wrongPassword")).ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task throw_exception_if_password_is_empty_for_user_with_unset_password()
        {
            await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource("notSetPassword:")).ShouldThrowAsync<ArgumentException>();
        }

        [Fact]
        public async Task connect_if_password_is_empty_for_user_with_empty_password()
        {
            using (await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource("emptyPassword:")))
            { }
        }

        [Fact]
        public async Task connect_with_credentials()
        {
            using (await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource("operator:operator")))
            { }
        }

        [Fact]
        public async Task do_nothing_if_already_connected()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource("operator:operator")))
            {
                var result = await box.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>("math.sqrt", TarantoolTuple.Create(1.3));

                var diff = Math.Abs(result.Data.Single().Item1 - Math.Sqrt(1.3));

                diff.ShouldBeLessThan(double.Epsilon);

                await box.Connect();

                var result2 = await box.Call_1_6<TarantoolTuple<double>, TarantoolTuple<double>>("math.sqrt", TarantoolTuple.Create(1.3));

                var diff2 = Math.Abs(result2.Data.Single().Item1 - Math.Sqrt(1.3));

                diff2.ShouldBeLessThan(double.Epsilon);
            }
        }

        [Fact]
        public async Task not_throw_expection_if_used_inside_another_class()
        {
            using (var box = await Client.Box.Connect(ConnectionStringFactory.GetReplicationSource("operator:operator")))
            using (var boxUser = new BoxUser(box))
            {
                var result = await boxUser.TestMethod();
                var diff = Math.Abs(result - Math.Sqrt(1.3));

                diff.ShouldBeLessThan(double.Epsilon);
            }
        }

        [Fact]
        public async Task change_IsConnected_state()
        {
            using (var box = new Client.Box(new ClientOptions(ConnectionStringFactory.GetReplicationSource("operator:operator"))))
            {
                box.IsConnected.ShouldBeFalse();
                await box.Connect();
                box.IsConnected.ShouldBeTrue();
            }
        }
    }
}
