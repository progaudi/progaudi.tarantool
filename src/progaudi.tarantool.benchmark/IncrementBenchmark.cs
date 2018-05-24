using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model.Responses;
using StackExchange.Redis;

namespace progaudi.tarantool.benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class IncrementBenchmark
    {
        private readonly Box _box;
        private readonly IDatabaseAsync _redis;

        public IncrementBenchmark()
        {
            _box = Box.Connect("localhost", 3301).GetAwaiter().GetResult();
            _redis = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase();
        }

        [Benchmark(Baseline = true)]
        public async Task<long> Redis() => await _redis.StringIncrementAsync("test_for_benchmarking");

        [Benchmark]
        public async Task<DataResponse<int[]>> Tarantool() => await _box.Call<int, int>("test_for_benchmarking", 0);
    }
}