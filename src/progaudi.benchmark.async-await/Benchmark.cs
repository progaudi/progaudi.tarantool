using System.Threading.Tasks;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Jobs;
using ProGaudi.Tarantool.Client;
using ProGaudi.Tarantool.Client.Model.Responses;

namespace ProGaudi.Tarantool.Benchmark.Async_Await
{
    [CoreJob]
    public class Benchmark
    {
        private readonly Box _box;

        public Benchmark()
        {
            this._box = Box.Connect("localhost", 3301).GetAwaiter().GetResult();
        }

        [Benchmark]
        public async Task<DataResponse<int[]>> Call() => await _box.Call<int>("test_for_benchmarking");
    }
}
