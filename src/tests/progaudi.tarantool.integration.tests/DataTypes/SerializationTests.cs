using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using Xunit;

namespace progaudi.tarantool.integration.tests.DataTypes
{
    /// <summary>
    /// Test suite, where we check correct data types serialization, when we pass values into Eval command,
    /// and also check that this value deserialize into correspoding C# class/structure correctly
    /// </summary>
    public class SerializationTests : TarantoolBaseTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task SerializeBoolean_ShouldBeCorrectAsync(bool val)
        {
            await AssertThatYouGetWhatYouGive(val);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(-1)]
        [InlineData(1000000)]
        public async Task SerializeInt_ShouldBeCorrectAsync(int val)
        {
            await AssertThatYouGetWhatYouGive(val);
        }

        [Theory]
        [InlineData("test")]
        public async Task SerializeString_ShouldBeCorrectAsync(string val)
        {
            await AssertThatYouGetWhatYouGive(val);
        }

        [Fact]
        public async Task SerializeGuid_ShouldBeCorrectAsync()
        {
            await AssertThatYouGetWhatYouGive(Guid.NewGuid());
        }

        [Theory]
        [InlineData("0")]
        [InlineData("100500")]
        [InlineData("0.1234567890123456789012345678")]
        public async Task SerializeDecimal_ShouldBeCorrectAsync(string val)
        {
            decimal n = Decimal.Parse(val);
            await AssertThatYouGetWhatYouGive(n);
            await AssertThatYouGetWhatYouGive(-n);
        }

        [Fact]
        public async Task SerializeDatetime_ShouldBeCorrectAsync()
        {
            var dt = DateTime.UtcNow;
            await AssertThatYouGetWhatYouGive(dt);
            await AssertThatYouGetWhatYouGive((DateTimeOffset)dt);
        }

        private static async Task AssertThatYouGetWhatYouGive<T>(T val)
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<TarantoolTuple<T>, T>($"return ...", TarantoolTuple.Create(val));
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBe(val);
        }
    }
}
