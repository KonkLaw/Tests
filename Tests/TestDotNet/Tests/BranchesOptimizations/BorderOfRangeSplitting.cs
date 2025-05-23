using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.BranchesOptimizations;

public class BorderOfRangeSplitting
{
    private const int ArraySize = 10_000;
    private const int maxInt = 10;
    private readonly float[] floats;

    public BorderOfRangeSplitting()
    {
        floats = RandomHelper.GetFloatNumbers(ArraySize, 0, maxInt);
        floats[RandomHelper.GetIntNumber(0, ArraySize)] = maxInt;
    }

    [Benchmark]
    public int WithIfProcessing()
    {
        float[] data = floats;
        int sum = 0;

        for (int i = 0; i < data.Length; i++)
        {
            int n = (int)data[i];
            if (n == maxInt)
                --n;
            sum += n;
        }
        return sum;
    }

    [Benchmark]
    public int WithMathProcessing()
    {
        float[] data = floats;
        int sum = 0;

        for (int i = 0; i < data.Length; i++)
        {
            int n = (int)data[i];
            n -= n / maxInt;
            sum += n;
        }
        return sum;
    }
}


//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.26100
//13th Gen Intel Core i5-13600K, 1 CPU, 20 logical and 14 physical cores
//.NET SDK= 9.0.203
//  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//
//
//|             Method |     Mean |     Error |    StdDev |
//|------------------- |---------:|----------:|----------:|
//|   WithIfProcessing | 3.141 us | 0.0026 us | 0.0020 us |
//| WithMathProcessing | 6.991 us | 0.0071 us | 0.0056 us |