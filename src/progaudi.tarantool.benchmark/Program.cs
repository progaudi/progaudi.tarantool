using BenchmarkDotNet.Running;

namespace progaudi.tarantool.benchmark
{
    public static class Program
    {
        public static void Main() => BenchmarkRunner.Run<IncrementBenchmark>();
    }
}
