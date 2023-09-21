using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
//Frequency = 2435957 Hz, Resolution = 410.5163 ns, Timer = TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//           Method | Mean        | StdDev      |
// -----------------| ------------| ----------- |
//  Sum_slow        | 202.3664 ms | 15.7631 ms  |
//  Sum_WithCaching | 151.1016 ms | 0.2391 ms   |


//----------------------------------------


//  BenchmarkDotNet=v0.13.1, OS=Windows 10.0.22621
//  13th Gen Intel Core i5-13600K, 1 CPU, 20 logical and 14 physical cores
//  .NET SDK=7.0.400
//  [Host]     : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT
//  DefaultJob : .NET 6.0.21 (6.0.2123.36311), X64 RyuJIT
//|
//|                    Method |     Mean |    Error |   StdDev |   Median |
//|-------------------------- |---------:|---------:|---------:|---------:|
//|                  Sum_Slow | 63.68 ms | 1.867 ms | 5.505 ms | 63.08 ms |
//|           Sum_WithCaching | 52.80 ms | 1.065 ms | 3.142 ms | 52.79 ms |
//|                  Sum_Span | 57.65 ms | 1.442 ms | 4.250 ms | 58.60 ms |
//|                Sum_Unsafe | 52.93 ms | 1.324 ms | 3.903 ms | 52.92 ms |

//|            Aggregate_Slow | 47.68 ms | 0.392 ms | 0.306 ms | 47.56 ms |
//|         Aggregate_Foreach | 43.96 ms | 1.063 ms | 3.134 ms | 42.93 ms |
//|         Aggregate_Caching | 45.10 ms | 0.953 ms | 2.810 ms | 44.88 ms |
//| Aggregate_Caching_Foreach | 43.83 ms | 0.874 ms | 2.492 ms | 43.13 ms |
//|            Aggregate_Span | 44.81 ms | 1.109 ms | 3.271 ms | 44.37 ms |
//|    Aggregate_Span_Foreach | 42.91 ms | 0.857 ms | 2.303 ms | 41.93 ms |
//|          Aggregate_Unsafe | 41.59 ms | 0.480 ms | 0.401 ms | 41.42 ms |

namespace TestDotNet.Tests.CollectionsTest;

public class WorkWitArrayTest
{
    private const int Count = 100_000_000;
    private readonly int[] a;
    private readonly int[] b;
    private readonly int[] result;

    public WorkWitArrayTest()
    {
        a = RandomHelper.GetIntNumbers(Count);
        b = RandomHelper.GetIntNumbers(Count);
        result = new int[Count];
    }

    [Benchmark]
    public int[] Sum_Slow()
    {
        for (int i = 0; i < Count; i++)
        {
            result[i] = a[i] + b[i];
        }
        return result;
    }

    [Benchmark]
    public int[] Sum_WithCaching()
    {
        // In this  case caching is REALLY important.
        int[] a_ = a;
        int[] b_ = b;
        int[] res_ = result;

        for (int i = 0; i < a_.Length; i++)
        {
            res_[i] = a_[i] + b_[i];
        }
        return res_;
    }

    [Benchmark]
    public int[] Sum_Span()
    {
        // In this  case caching is REALLY important.
        ReadOnlySpan<int> a_ = a.AsSpan();
        ReadOnlySpan<int> b_ = b.AsSpan();
        Span<int> res2_ = result.AsSpan();
        for (int i = 0; i < a_.Length; i++)
        {
            res2_[i] = a_[i] + b_[i];
        }
        return result;
    }

    [Benchmark]
    public unsafe int[] Sum_Unsafe()
    {
        fixed (int* aP = a)
        {
            fixed (int* bP = b)
            {
                fixed (int* resultP = result)
                {
                    for (int i = 0; i < a.Length; i++)
                    {
                        resultP[i] = aP[i] + bP[i];
                    }
                }
            }
        }
        return result;
    }

    [Benchmark]
    public long Aggregate_Slow()
    {
        long res = 0;
        for (int i = 0; i < a.Length; i++)
        {
            res += a[i];
        }
        return res;
    }

    [Benchmark]
    public long Aggregate_Foreach()
    {
        long res = 0;
        foreach (int q in a)
        {
            res += q;
        }
        return res;
    }

    [Benchmark]
    public long Aggregate_Caching()
    {
        int[] a_ = a;
        long res = 0;
        for (int i = 0; i < a_.Length; i++)
        {
            res += a_[i];
        }
        return res;
    }

    [Benchmark]
    public long Aggregate_Caching_Foreach()
    {
        int[] a_ = a;
        long res = 0;
        foreach (int q in a_)
        {
            res += q;
        }
        return res;
    }

    [Benchmark]
    public long Aggregate_Span()
    {
        ReadOnlySpan<int> a_ = a.AsSpan();
        long res = 0;
        for (int i = 0; i < a_.Length; i++)
        {
            res += a_[i];
        }
        return res;
    }

    [Benchmark]
    public long Aggregate_Span_Foreach()
    {
        ReadOnlySpan<int> a_ = a.AsSpan();
        long res = 0;
        foreach (int q in a_)
        {
            res += q;
        }
        return res;
    }

    [Benchmark]
    public unsafe long Aggregate_Unsafe()
    {
        long res = 0;
        int[] a_ = a;
        fixed (int* ptr = a)
        {
            for (int i = 0; i < a_.Length; i++)
            {
                res += ptr[i];
            }
        }
        return res;
    }
}