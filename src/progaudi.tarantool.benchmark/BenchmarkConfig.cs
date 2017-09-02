using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Environments;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Exporters.Csv;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.CsProj;

namespace progaudi.tarantool.benchmark
{
    internal class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(StatisticColumn.P95);

            Add(MemoryDiagnoser.Default);

            // https://github.com/dotnet/BenchmarkDotNet/issues/500
            //Add(Job.ShortRun.With(Jit.LegacyJit).With(Platform.X86).With(Runtime.Clr));
            //Add(Job.ShortRun.With(Jit.LegacyJit).With(Platform.X64).With(Runtime.Clr));

            //Add(Job.ShortRun.With(Jit.RyuJit).With(Platform.X64).With(Runtime.Clr));

            // RyuJit for .NET Core 1.1
            Add(Job.Default.With(Jit.RyuJit).With(Platform.X64).With(Runtime.Core).With(CsProjCoreToolchain.NetCoreApp11).WithId("netcore1.1"));

            // RyuJit for .NET Core 2.0
            Add(Job.Default.With(Jit.RyuJit).With(Platform.X64).With(Runtime.Core).With(CsProjCoreToolchain.NetCoreApp20).WithId("netcore2.0"));

            Add(MarkdownExporter.GitHub);
            Add(CsvMeasurementsExporter.Default);
        }
    }
}