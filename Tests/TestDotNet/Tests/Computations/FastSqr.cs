using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.Computations;

public class FastSqr
{
    private double random;

    public FastSqr()
    {
        random = RandomHelper.GetDoubleNumbers(1)[0];
    }

    [Benchmark]
    public double Multipl() => random * random;

    [Benchmark]
    public double MathPow() => Math.Pow(random, 2);
}
