using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

//BenchmarkDotNet=v0.10.7, OS=Windows 10.0.18363
//Processor=Intel Core i7-8750H CPU 2.20GHz, ProcessorCount=12
//Frequency=10000000 Hz, Resolution=100.0000 ns, Timer=UNKNOWN
//  [Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0
//  DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.8.4150.0


//          Method |     Mean |    Error |    StdDev |
//---------------- |---------:|---------:|----------:|
// ShortCheck_For3 | 442.3 us | 1.618 us | 1.4343 us |
//  FullCheck_For3 | 468.7 us | 1.329 us | 1.1777 us |
// ShortCheck_For2 | 431.0 us | 1.066 us | 0.9448 us |
//  FullCheck_For2 | 437.7 us | 1.714 us | 1.5196 us |
// ShortCheck_For5 | 511.9 us | 1.491 us | 1.3947 us |
//  FullCheck_For5 | 556.5 us | 1.636 us | 1.5303 us |

namespace TestDotNet.Tests.BranchesOptimizations;

public class InIfTest
{
    private const int ValuesCount = 100_000;

    private readonly int[] collectionFor2;
    private readonly int[] collectionFor3;
    private readonly int[] collectionFor5;

    public InIfTest()
    {
        collectionFor2 = GetFor2();
        collectionFor3 = GetFor3();
        collectionFor5 = GetFor5();
    }

    private int[] GetFor3()
    {
        var array = new int[ValuesCount];
        for (int i = 0; i < array.Length; i++)
        {
            int val = RandomHelper.GetIntNumber(-33, 168);
            // +:
            // (0 - -33) + (133 - 100) + ((168 - 1) - 133) = 33 + 33 + 34 = 100
            // -:
            // (50 - 0) + (100 - 50) = 100
            array[i] = val > 133 ? 50 : val;
        }
        return array;
    }

    [Benchmark]
    public int ShortCheck_For3()
    {
        int counter = 0;
        int[] values = collectionFor3;
        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            if (value < 0 || value == 50 || value > 100)
            {
                counter++;
            }
            else
            {
                counter--;
            }
        }
        return counter;
    }

    [Benchmark]
    public int FullCheck_For3()
    {
        int counter = 0;
        int[] values = collectionFor3;
        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            if (value < 0 | value == 50 | value > 100)
            {
                counter++;
            }
            else
            {
                counter--;
            }
        }
        return counter;
    }

    private int[] GetFor2()
    {
        var array = new int[ValuesCount];
        for (int i = 0; i < array.Length; i++)
        {
            int val = RandomHelper.GetIntNumber(0, 4);
            array[i] = val;
        }
        return array;
    }

    [Benchmark]
    public int ShortCheck_For2()
    {
        int counter = 0;
        int[] values = collectionFor2;
        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            if (value == 0 || value == 2)
            {
                counter++;
            }
            else
            {
                counter--;
            }
        }
        return counter;
    }

    [Benchmark]
    public int FullCheck_For2()
    {
        int counter = 0;
        int[] values = collectionFor2;
        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            if (value == 0 | value == 2)
            {
                counter++;
            }
            else
            {
                counter--;
            }
        }
        return counter;
    }

    private int[] GetFor5()
    {
        var array = new int[ValuesCount];
        for (int i = 0; i < array.Length; i++)
        {
            int val = RandomHelper.GetIntNumber(0, 10);
            array[i] = val;
        }
        return array;
    }

    [Benchmark]
    public int ShortCheck_For5()
    {
        int counter = 0;
        int[] values = collectionFor5;
        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            if (value == 0 || value == 2 || value == 4 || value == 6 || value == 8)
            {
                counter++;
            }
            else
            {
                counter--;
            }
        }
        return counter;
    }

    [Benchmark]
    public int FullCheck_For5()
    {
        int counter = 0;
        int[] values = collectionFor5;
        for (int i = 0; i < values.Length; i++)
        {
            int value = values[i];
            if (value == 0 | value == 2 | value == 4 | value == 6 | value == 8)
            {
                counter++;
            }
            else
            {
                counter--;
            }
        }
        return counter;
    }
}
