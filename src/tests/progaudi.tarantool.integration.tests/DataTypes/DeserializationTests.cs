using Shouldly;
using System.Text;
using NUnit.Framework;
using System.Globalization;

namespace progaudi.tarantool.integration.tests.DataTypes
{
    /// <summary>
    /// Test suite, where we create and return some values in Tarantool via Lua and Eval command, 
    /// and check that this value deserialize into correspoding C# class/structure correctly
    /// </summary>
    [TestFixture]
    public class DeserializationTests : TarantoolBaseTest
    {
        [Test]
        public async Task DeserializeNull_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<string>("return box.NULL");
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBeNull();
        }

        [Test]
        public async Task DeserializeNil_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<string>("return nil");
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBeNull();
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task DeserializeBoolean_ShouldBeCorrectAsync(bool val)
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<bool>($"return {(val ? "true" : "false")}");
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBe(val);
        }

        [TestCase(1)]
        [TestCase(0)]
        [TestCase(-1)]
        public async Task DeserializeInt_ShouldBeCorrectAsync(int val)
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<int>($"return {val}");
            result.Data.ShouldBe(new[] { val });
        }

        [Test]
        public async Task DeserializeFloat64_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<double>("return math.sqrt(2)");
            result.Data.Length.ShouldBe(1);
            Math.Abs(result.Data[0] - Math.Sqrt(2)).ShouldBeLessThan(double.Epsilon);
        }

        [Test]
        public async Task DeserializeString_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var expectedStr = "Tarantool tickles, makes spiders giggle";
            var result = await tarantoolClient.Eval<string>($"return '{expectedStr}'");
            result.Data.ShouldBe(new[] { expectedStr });
        }
  
        [TestCase("2.7182818284590452353602874714")]
        [TestCase("2.718281828459045235360287471")]
        [TestCase("2.71828182845904523536028747")]
        [TestCase("2.7182818284590452353602874")]
        [TestCase("2.718281828459045235360287")]
        [TestCase("2.71828182845904523536028")]
        [TestCase("2.7182818284590452353602")]
        [TestCase("2.718281828459045235360")]
        [TestCase("2.71828182845904523536")]
        [TestCase("2.7182818284590452353")]
        [TestCase("2.718281828459045235")]
        [TestCase("2.71828182845904523")]
        [TestCase("2.7182818284590452")]
        [TestCase("2.718281828459045")]
        [TestCase("2.71828182845904")]
        [TestCase("2.7182818284590")]
        [TestCase("2.718281828459")]
        [TestCase("2.71828182845")]
        [TestCase("2.7182818284")]
        [TestCase("2.718281828")]
        [TestCase("2.71828182")]
        [TestCase("2.7182818")]
        [TestCase("2.718281")]
        [TestCase("2.71828")]
        [TestCase("2.7182")]
        [TestCase("2.718")]
        [TestCase("2.71")]
        [TestCase("2.7")]
        [TestCase("2")]
        [TestCase("0")]
        [TestCase("100000")]
        [TestCase("0.1")]
        [TestCase("0.01")]
        [TestCase("0.001")]
        [TestCase("0.0001")]
        
        public async Task DeserializeExtAsDecimal_CorrectValue_ShouldBeDeserializedCorrectlyAsync(string str)
        {
            decimal n = Decimal.Parse(str, CultureInfo.InvariantCulture);
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<decimal>($"local decimal = require(\"decimal\"); return decimal.new(\"{str}\")");
            result.Data.ShouldBe(new[] { n });

            var negativeResult = await tarantoolClient.Eval<decimal>($"local decimal = require(\"decimal\"); return decimal.new(\"-{str}\")");
            negativeResult.Data.ShouldBe(new[] { -n });
        }

        [TestCase("0.12345678901234567890123456789")]// scale == 29 (max possible in .net is 28)
        [TestCase("0.12345678901234567890123456789012345678")]// scale == 38 (max possible in .net is 28)
        [TestCase("79228162514264337593543950336")]// max .net decimal + 1
        [TestCase("-79228162514264337593543950336")]// min .net decimal - 1
        public async Task DeserializeExtAsDecimal_IncorrectValue_OverflowExceptionThrown(string str)
        {
            using var tarantoolClient = await GetTarantoolClient();
            
            Assert.ThrowsAsync<OverflowException>(async () =>
                await tarantoolClient.Eval<decimal>($"local decimal = require(\"decimal\"); return decimal.new(\"{str}\")"));
        }

        [Test]
        public async Task DeserializeBinary8_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<byte[]>($"local msgpack = require(\"msgpack\"); return msgpack.object_from_raw('\\xc4\\x06foobar')");
            var expectedByteArray = Encoding.ASCII.GetBytes("foobar");
            result.Data.ShouldBe(new[] { expectedByteArray });
        }

        [Test]
        public async Task DeserializeBinary16_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var stringLen256 = String.Join("", Enumerable.Range(0, 256).Select(_ => "x"));
            var result = await tarantoolClient.Eval<byte[]>($"local msgpack = require(\"msgpack\"); return msgpack.object_from_raw('\\xc5\\x01\\x00{stringLen256}')");
            var expectedByteArray = Encoding.ASCII.GetBytes(stringLen256);
            result.Data.ShouldBe(new[] { expectedByteArray });
        }

        [Test]
        public async Task DeserializeExtAsGuid_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var guid = new Guid();
            var result = await tarantoolClient.Eval<Guid>($"local uuid = require(\"uuid\"); return uuid.fromstr(\"{guid}\")");
            result.Data.ShouldBe(new[] { guid });
        }

        [Test]
        public async Task DeserializeExtAsDatetime_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var dt = DateTime.UtcNow;
            var query = $"local dt = require(\"datetime\"); " +
                $"return dt.new {{ msec = {dt.Millisecond}, sec = {dt.Second}, min = {dt.Minute}, hour = {dt.Hour}, day = {dt.Day}, month = {dt.Month}, year = {dt.Year} }}";
            // TODO: test tzoffset, nsec/usec additionally
            var result = await tarantoolClient.Eval<DateTime>(query);
            result.Data.Length.ShouldBe(1);
            var actualDt = result.Data[0];
            actualDt.Date.ShouldBe(dt.Date);
            actualDt.Hour.ShouldBe(dt.Hour);
            actualDt.Minute.ShouldBe(dt.Minute);
            actualDt.Second.ShouldBe(dt.Second);
            actualDt.Millisecond.ShouldBe(dt.Millisecond);

            var resultOffset = await tarantoolClient.Eval<DateTimeOffset>(query);
            resultOffset.Data.Length.ShouldBe(1);
            var actualOffset = resultOffset.Data[0];
            actualDt.Date.ShouldBe(dt.Date);
            actualDt.Hour.ShouldBe(dt.Hour);
            actualDt.Minute.ShouldBe(dt.Minute);
            actualDt.Second.ShouldBe(dt.Second);
            actualDt.Millisecond.ShouldBe(dt.Millisecond);
        }

        [Test]
        public async Task DeserializeIntArray_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new int[3] { 1, 2, 3};
            var result = await tarantoolClient.Eval<int[]>($"return {{{String.Join(",", arr)}}}");
            result.Data.ShouldBe(new[] { arr });
        }

        [Test]
        public async Task DeserializeBooleanArray_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new bool[3] { false, true, false };
            var result = await tarantoolClient.Eval<bool[]>($"return {{{String.Join(",", arr.Select(x => x.ToString().ToLower()))}}}");
            result.Data.ShouldBe(new[] { arr });
        }

        [Test]
        public async Task DeserializeFloat64Array_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new double[3] { Math.Sqrt(2), Math.Sqrt(3), Math.Sqrt(5) };
            var result = await tarantoolClient.Eval<double[]>("return { math.sqrt(2), math.sqrt(3), math.sqrt(5) }");
            result.Data.Length.ShouldBe(1);
            result.Data[0].Length.ShouldBe(3);
            Math.Abs(result.Data[0][0] - Math.Sqrt(2)).ShouldBeLessThan(double.Epsilon);
            Math.Abs(result.Data[0][1] - Math.Sqrt(3)).ShouldBeLessThan(double.Epsilon);
            Math.Abs(result.Data[0][2] - Math.Sqrt(5)).ShouldBeLessThan(double.Epsilon);
        }

        [Test]
        public async Task DeserializeStringArray_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new string[3] { "foo", "bar", "foobar" };
            var result = await tarantoolClient.Eval<string[]>($"return {{{String.Join(",", arr.Select(x => "'" + x + "'"))}}}");
            result.Data.ShouldBe(new[] { arr });
        }

        [Test]
        public async Task DeserializeMixedArrayToTuple_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<Tuple<int, bool, string>>("return { 1, true, 'foo'}");
            result.Data.Length.ShouldBe(1);
            result.Data[0].Item1.ShouldBe(1);
            result.Data[0].Item2.ShouldBe(true);
            result.Data[0].Item3.ShouldBe("foo");
        }

        [Test]
        public async Task DeserializeMapToDictionary_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var expectedDict = new Dictionary<string, int>() 
            {
                { "foo", 1 },
                { "bar", 2 },
                { "baz", 3 }
            };
            var result = await tarantoolClient.Eval<Dictionary<string, int>>($"a = {{}}; {String.Join("; ", expectedDict.Select(kvp => $"a['{kvp.Key}']={kvp.Value}"))} return a");
            result.Data.Length.ShouldBe(1);
            var actualDict = result.Data[0];
            foreach (var key in expectedDict.Keys) // order doesn't preserve, so we need to check key by key
            {
                actualDict.ContainsKey(key).ShouldBeTrue();
                actualDict[key].ShouldBe(expectedDict[key]);
            }
        }
    }
}