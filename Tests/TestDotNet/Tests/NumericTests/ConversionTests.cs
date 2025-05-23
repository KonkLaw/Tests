using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

namespace TestDotNet.Tests.NumericTests;

public class ConversionTests
{
    private float[] numbers;
    private int[] testData;

    [IterationSetup]
    public void Setup()
    {
        const int arraySize = 100_000_000;
        numbers = RandomHelper.GetFloatNumbers(arraySize, -1000, 1000);
        testData = RandomHelper.GetIntNumbers(arraySize, -1000, 1000);
    }

    [Benchmark]
    public int TestRun()
    {
        int sum = 0;
        int[] array = testData;
        for (int i = 0; i < array.Length; i++)
        {
            sum += array[i];
        }
        return sum;
    }

    [Benchmark]
    public int Float_ToInt()
    {
        int sum = 0;
        float[] array = numbers;
        for (int i = 0; i < array.Length; i++)
        {
            sum += (int)array[i];
        }

        return sum;
    }

    [Benchmark]
    public int Float_Floor_ToInt()
    {
        int sum = 0;
        float[] array = numbers;
        for (int i = 0; i < array.Length; i++)
        {
            sum += (int)Math.Floor(array[i]);
        }

        return sum;
    }

    [Benchmark]
    public int Float_Ceiling_ToInt()
    {
        int sum = 0;
        float[] array = numbers;
        for (int i = 0; i < array.Length; i++)
        {
            sum += (int)Math.Floor(array[i]);
        }

        return sum;
    }
}

//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.26100
//13th Gen Intel Core i5-13600K, 1 CPU, 20 logical and 14 physical cores
//.NET SDK= 9.0.203
//  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//  Job-HOUKNM : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//
//InvocationCount=1  UnrollFactor=1
//
//|              Method |      Mean |    Error |   StdDev |    Median |
//|-------------------- |----------:|---------:|---------:|----------:|
//|             TestRun |  34.76 ms | 0.787 ms | 2.321 ms |  34.85 ms |
//|         Float_ToInt |  39.43 ms | 1.275 ms | 3.760 ms |  41.16 ms |
//|   Float_Floor_ToInt | 259.60 ms | 1.795 ms | 1.679 ms | 258.90 ms |
//| Float_Ceiling_ToInt | 259.64 ms | 1.472 ms | 1.377 ms | 259.58 ms |