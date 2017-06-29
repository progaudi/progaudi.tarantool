``` ini

BenchmarkDotNet=v0.10.7, OS=Windows 10 Redstone 2 (10.0.15063)
Processor=Intel Core i5-4590 CPU 3.30GHz (Haswell), ProcessorCount=4
Frequency=3215209 Hz, Resolution=311.0218 ns, Timer=TSC
dotnet cli version=1.0.0
  [Host] : .NET Core 4.6.25211.01, 64bit RyuJIT
  Core   : .NET Core 4.6.25211.01, 64bit RyuJIT

Job=Core  Runtime=Core  

```
 | Method |     Mean |    Error |   StdDev |
 |------- |---------:|---------:|---------:|
 |   Call | 448.1 us | 16.68 us | 47.86 us |
