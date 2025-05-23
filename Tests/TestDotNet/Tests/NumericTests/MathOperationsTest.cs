//BenchmarkDotNet=v0.12.1, OS=Windows 10.0.19042
//Intel Core i7-8750H CPU 2.20GHz (Coffee Lake), 1 CPU, 12 logical and 6 physical cores
//  [Host]     : .NET Framework 4.8 (4.8.4341.0), X64 RyuJIT
//  DefaultJob : .NET Framework 4.8 (4.8.4341.0), X64 RyuJIT
//
//
//|        Method |       Mean |     Error |    StdDev |
//|-------------- |-----------:|----------:|----------:|
//|           Tan | 47.7281 ns | 0.1239 ns | 0.1034 ns |
//| MultiplyFloat |  0.0597 ns | 0.0021 ns | 0.0019 ns |
//|      AddFloat |  0.0669 ns | 0.0171 ns | 0.0151 ns |

using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.NumericTests;

public class MathOperationsTest
{
    public float value1;
    public float value2;
    public double valueD;

    public MathOperationsTest()
    {
        value1 = RandomHelper.GetFloat();
        value2 = RandomHelper.GetFloat();
        valueD = RandomHelper.GetFloat();
    }

    [Benchmark]
    public double Tan()
    {
        return Math.Tan(valueD);
    }

    [Benchmark]
    public float MultiplyFloat()
    {
        return value1 * value2;
    }

    [Benchmark]
    public float AddFloat()
    {
        return value1 + value2;
    }
}
