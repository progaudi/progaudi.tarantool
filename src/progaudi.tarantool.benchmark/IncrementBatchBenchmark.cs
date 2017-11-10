using System;
using System.Linq;
using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using ProGaudi.Tarantool.Client;
using StackExchange.Redis;

namespace progaudi.tarantool.benchmark
{
    [Config(typeof(BenchmarkConfig))]
    public class IncrementBatchBenchmark
    {
        private readonly Box _box;
        private readonly IDatabaseAsync _redis;
        private readonly int _batchSize;
        private readonly ISpace _space;

        public IncrementBatchBenchmark()
        {
            _box = Box.Connect("localhost", 3301).GetAwaiter().GetResult();
            _space = _box.Schema["benchmark"];
            _redis = ConnectionMultiplexer.Connect("localhost:6379").GetDatabase();
            _batchSize = 1000;
        }

        [Benchmark(Baseline = true)]
        public async Task Redis() => await Task.WhenAll(Enumerable.Range(0, _batchSize).Select(_ => _redis.StringIncrementAsync("test_for_benchmarking")).ToArray());

        [Benchmark]
        public async Task Call() => await Task.WhenAll(Enumerable.Range(0, _batchSize).Select(_ => _box.Call<int>("test_for_benchmarking")).ToArray());

        [Benchmark]
        public async Task Select() => await Task.WhenAll(Enumerable.Range(0, _batchSize).Select(_ => _space.Select<ValueTuple<int>, (int, int, string, string, string)>(ValueTuple.Create(1))).ToArray());
    }
}