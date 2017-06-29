using BenchmarkDotNet.Running;

namespace ProGaudi.Tarantool.Benchmark.Async_Await
{
    public static class Program
    {
        public static void Main() => BenchmarkRunner.Run<Benchmark>();
    }
}