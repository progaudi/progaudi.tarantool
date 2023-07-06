using ProGaudi.Tarantool.Client.Model;
using Shouldly;
using NUnit.Framework;
using System.Globalization;

namespace progaudi.tarantool.integration.tests.DataTypes
{
    /// <summary>
    /// Test suite, where we check correct data types serialization, when we pass values into Eval command,
    /// and also check that this value deserialize into correspoding C# class/structure correctly
    /// </summary>
    [TestFixture]
    public class SerializationTests : TarantoolBaseTest
    {
        [TestCase(true)]
        [TestCase(false)]
        public async Task SerializeBoolean_ShouldBeCorrectAsync(bool val)
        {
            await AssertThatYouGetWhatYouGive(val);
        }
        
        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(1000000)]
        public async Task SerializeInt_ShouldBeCorrectAsync(int val)
        {
            await AssertThatYouGetWhatYouGive(val);
        }

        [TestCase("test")]
        public async Task SerializeString_ShouldBeCorrectAsync(string val)
        {
            await AssertThatYouGetWhatYouGive(val);
        }

        [Test]
        public async Task SerializeGuid_ShouldBeCorrectAsync()
        {
            await AssertThatYouGetWhatYouGive(Guid.NewGuid());
        }

        [TestCase("0")]
        [TestCase("100500")]
        [TestCase("0.1234567890123456789012345678")]
        public async Task SerializeDecimal_ShouldBeCorrectAsync(string val)
        {
            decimal n = Decimal.Parse(val, CultureInfo.InvariantCulture);
            await AssertThatYouGetWhatYouGive(n);
            await AssertThatYouGetWhatYouGive(-n);
        }

        [Test]
        public async Task SerializeDatetime_ShouldBeCorrectAsync()
        {
            var dt = DateTime.UtcNow;
            await AssertThatYouGetWhatYouGive(dt);
            await AssertThatYouGetWhatYouGive((DateTimeOffset)dt);
        }

        [Test]
        public async Task SerializeTuple_ShouldBeCorrectAsync()
        {
            await AssertThatYouGetWhatYouGive(Tuple.Create(1, true, "test", 1m));
        }

        [Test]
        public async Task SerializeDictionary_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var expectedDict = new Dictionary<string, int>()
            {
                { "foo", 1 },
                { "bar", 2 },
                { "baz", 3 }
            };
            var result = await tarantoolClient.Eval<TarantoolTuple<Dictionary<string, int>>, Dictionary<string, int>>($"return ...", TarantoolTuple.Create(expectedDict));
            
            result.Data.Length.ShouldBe(1);
            var actualDict = result.Data[0];
            foreach (var key in expectedDict.Keys) // order doesn't preserve, so we need to check key by key
            {
                actualDict.ContainsKey(key).ShouldBeTrue();
                actualDict[key].ShouldBe(expectedDict[key]);
            }
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
