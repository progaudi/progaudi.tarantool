using Xunit;
using Shouldly;
using System.Text;

namespace progaudi.tarantool.integration.tests.DataTypes
{
    /// <summary>
    /// Test suite, where we create and return some values in Tarantool via Lua and Eval command, 
    /// and check that this value deserialize into correspoding C# class/structure correctly
    /// </summary>
    public class DeserializationTests : TarantoolBaseTest
    {
        [Fact]
        public async Task DeserializeNull_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<string>("return box.NULL");
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBeNull();
        }

        [Fact]
        public async Task DeserializeNil_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<string>("return nil");
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBeNull();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task DeserializeBoolean_ShouldBeCorrectAsync(bool val)
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<bool>($"return {(val ? "true" : "false")}");
            result.Data.Length.ShouldBe(1);
            result.Data[0].ShouldBe(val);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(0)]
        [InlineData(-1)]
        public async Task DeserializeInt_ShouldBeCorrectAsync(int val)
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<int>($"return {val}");
            result.Data.ShouldBe(new[] { val });
        }

        [Fact]
        public async Task DeserializeFloat64_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<double>("return math.sqrt(2)");
            result.Data.Length.ShouldBe(1);
            Math.Abs(result.Data[0] - Math.Sqrt(2)).ShouldBeLessThan(double.Epsilon);
        }

        [Fact]
        public async Task DeserializeString_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var expectedStr = "Tarantool tickles, makes spiders giggle";
            var result = await tarantoolClient.Eval<string>($"return '{expectedStr}'");
            result.Data.ShouldBe(new[] { expectedStr });
        }

        [Theory]      
        [InlineData("2.7182818284590452353602874714")]
        [InlineData("2.718281828459045235360287471")]
        [InlineData("2.71828182845904523536028747")]
        [InlineData("2.7182818284590452353602874")]
        [InlineData("2.718281828459045235360287")]
        [InlineData("2.71828182845904523536028")]
        [InlineData("2.7182818284590452353602")]
        [InlineData("2.718281828459045235360")]
        [InlineData("2.71828182845904523536")]
        [InlineData("2.7182818284590452353")]
        [InlineData("2.718281828459045235")]
        [InlineData("2.71828182845904523")]
        [InlineData("2.7182818284590452")]
        [InlineData("2.718281828459045")]
        [InlineData("2.71828182845904")]
        [InlineData("2.7182818284590")]
        [InlineData("2.718281828459")]
        [InlineData("2.71828182845")]
        [InlineData("2.7182818284")]
        [InlineData("2.718281828")]
        [InlineData("2.71828182")]
        [InlineData("2.7182818")]
        [InlineData("2.718281")]
        [InlineData("2.71828")]
        [InlineData("2.7182")]
        [InlineData("2.718")]
        [InlineData("2.71")]
        [InlineData("2.7")]
        [InlineData("2")]
        [InlineData("0")]
        [InlineData("100000")]
        [InlineData("0.1")]
        [InlineData("0.01")]
        [InlineData("0.001")]
        [InlineData("0.0001")]
        public async Task DeserializeExtAsDecimal_ShouldBeCorrectAsync(string str)
        {
            decimal n = Decimal.Parse(str);
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<decimal>($"local decimal = require(\"decimal\"); return decimal.new(\"{n}\")");
            result.Data.ShouldBe(new[] { n });

            var negativeResult = await tarantoolClient.Eval<decimal>($"local decimal = require(\"decimal\"); return decimal.new(\"{-n}\")");
            negativeResult.Data.ShouldBe(new[] { -n });
        }

        [Fact]
        public async Task DeserializeBinary8_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<byte[]>($"local msgpack = require(\"msgpack\"); return msgpack.object_from_raw('\\xc4\\x06foobar')");
            var expectedByteArray = Encoding.ASCII.GetBytes("foobar");
            result.Data.ShouldBe(new[] { expectedByteArray });
        }

        [Fact]
        public async Task DeserializeBinary16_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var stringLen256 = String.Join("", Enumerable.Range(0, 256).Select(_ => "x"));
            var result = await tarantoolClient.Eval<byte[]>($"local msgpack = require(\"msgpack\"); return msgpack.object_from_raw('\\xc5\\x01\\x00{stringLen256}')");
            var expectedByteArray = Encoding.ASCII.GetBytes(stringLen256);
            result.Data.ShouldBe(new[] { expectedByteArray });
        }

        [Fact]
        public async Task DeserializeExtAsGuid_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var guid = new Guid();
            var result = await tarantoolClient.Eval<Guid>($"local uuid = require(\"uuid\"); return uuid.fromstr(\"{guid}\")");
            result.Data.ShouldBe(new[] { guid });
        }

        [Fact]
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

        [Fact]
        public async Task DeserializeIntArray_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new int[3] { 1, 2, 3};
            var result = await tarantoolClient.Eval<int[]>($"return {{{String.Join(",", arr)}}}");
            result.Data.ShouldBe(new[] { arr });
        }

        [Fact]
        public async Task DeserializeBooleanArray_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new bool[3] { false, true, false };
            var result = await tarantoolClient.Eval<bool[]>($"return {{{String.Join(",", arr.Select(x => x.ToString().ToLower()))}}}");
            result.Data.ShouldBe(new[] { arr });
        }

        [Fact]
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

        [Fact]
        public async Task DeserializeStringArray_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var arr = new string[3] { "foo", "bar", "foobar" };
            var result = await tarantoolClient.Eval<string[]>($"return {{{String.Join(",", arr.Select(x => "'" + x + "'"))}}}");
            result.Data.ShouldBe(new[] { arr });
        }

        [Fact]
        public async Task DeserializeMixedArrayToTuple_ShouldBeCorrectAsync()
        {
            using var tarantoolClient = await GetTarantoolClient();
            var result = await tarantoolClient.Eval<Tuple<int, bool, string>>("return { 1, true, 'foo'}");
            result.Data.Length.ShouldBe(1);
            result.Data[0].Item1.ShouldBe(1);
            result.Data[0].Item2.ShouldBe(true);
            result.Data[0].Item3.ShouldBe("foo");
        }

        [Fact]
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