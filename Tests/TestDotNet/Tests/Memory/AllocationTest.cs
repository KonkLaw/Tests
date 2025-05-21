using System.Runtime;
using System.Runtime.InteropServices;
using BenchmarkDotNet.Attributes;
using Iced.Intel;

namespace TestDotNet.Tests.Memory;

public class AllocationTest
{
    [Params(100, 1_000, 60_000, 100_000)]
    public uint ArraySize { get; set; }

    [Params(10, 1_000, 20_000, 100_000)]
    public int AllocationsCount { get; set; }

    [Benchmark]
    public unsafe void NativeAllocTests()
    {
        void*[] array = new void*[AllocationsCount];
        for (var i = 0; i < array.Length; i++)
        {
            void* ptr = NativeMemory.Alloc(ArraySize);
            array[i] = ptr;
        }

        for (var i = 0; i < array.Length; i++)
        {
            void* ptr = array[i];
            NativeMemory.Free(ptr);
        }
    }

    [Benchmark]
    public void GCAllocTests_NoLohCompact()
    {
        RunWithGC();

        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    [Benchmark]
    public void GCAllocTests_LohCompact()
    {
        RunWithGC();

        GCSettings.LargeObjectHeapCompactionMode = GCLargeObjectHeapCompactionMode.CompactOnce;
        GC.Collect();
        GC.WaitForPendingFinalizers();
    }

    private void RunWithGC()
    {
        byte[][] array = new byte[AllocationsCount][];
        for (var i = 0; i < array.Length; i++)
        {
            byte[] ptr = new byte[ArraySize];
            array[i] = ptr;
        }

        GC.KeepAlive(array);
    }
}



//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.26100
//13th Gen Intel Core i5-13600K, 1 CPU, 20 logical and 14 physical cores
//.NET SDK= 9.0.203
//  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//
//
//|                    Method | ArraySize | AllocationsCount |             Mean |            Error |           StdDev |           Median |
//|-------------------------- |---------- |----------------- |-----------------:|-----------------:|-----------------:|-----------------:|
//|          NativeAllocTests |       100 |               10 |         241.7 ns |          1.07 ns |          0.83 ns |         241.5 ns |
//| GCAllocTests_NoLohCompact |       100 |               10 |      40,435.4 ns |        250.69 ns |        209.34 ns |      40,444.0 ns |
//|   GCAllocTests_LohCompact |       100 |               10 |      63,627.4 ns |      1,198.88 ns |      1,231.16 ns |      63,934.8 ns |
//|          NativeAllocTests |       100 |             1000 |      26,912.6 ns |        375.38 ns |        332.76 ns |      26,800.0 ns |
//| GCAllocTests_NoLohCompact |       100 |             1000 |      46,729.2 ns |        888.77 ns |      1,124.01 ns |      46,777.4 ns |
//|   GCAllocTests_LohCompact |       100 |             1000 |      67,424.3 ns |      1,275.92 ns |      1,193.50 ns |      67,474.7 ns |
//|          NativeAllocTests |       100 |            20000 |     586,161.1 ns |      6,685.80 ns |      6,253.90 ns |     584,877.3 ns |
//| GCAllocTests_NoLohCompact |       100 |            20000 |     206,072.4 ns |        548.28 ns |        428.06 ns |     206,120.1 ns |
//|   GCAllocTests_LohCompact |       100 |            20000 |     210,897.9 ns |      4,208.30 ns |      5,471.97 ns |     209,430.0 ns |
//|          NativeAllocTests |       100 |           100000 |   3,129,578.5 ns |     40,546.07 ns |     35,943.04 ns |   3,134,057.0 ns |
//| GCAllocTests_NoLohCompact |       100 |           100000 |   2,114,793.9 ns |     41,928.93 ns |    114,070.76 ns |   2,128,195.9 ns |
//|   GCAllocTests_LohCompact |       100 |           100000 |   2,209,527.1 ns |     44,141.00 ns |    129,457.90 ns |   2,229,523.6 ns |
//|          NativeAllocTests |      1000 |               10 |         244.6 ns |          0.36 ns |          0.28 ns |         244.6 ns |
//| GCAllocTests_NoLohCompact |      1000 |               10 |      40,024.2 ns |        165.20 ns |        137.95 ns |      40,058.9 ns |
//|   GCAllocTests_LohCompact |      1000 |               10 |      63,118.2 ns |      1,225.32 ns |      1,311.08 ns |      63,246.0 ns |
//|          NativeAllocTests |      1000 |             1000 |      37,109.1 ns |        314.57 ns |        278.86 ns |      37,078.8 ns |
//| GCAllocTests_NoLohCompact |      1000 |             1000 |      87,805.8 ns |      1,737.20 ns |      1,450.64 ns |      88,063.6 ns |
//|   GCAllocTests_LohCompact |      1000 |             1000 |      87,674.6 ns |      1,357.25 ns |      1,269.57 ns |      87,934.1 ns |
//|          NativeAllocTests |      1000 |            20000 |   1,117,888.7 ns |    103,986.40 ns |    306,606.24 ns |   1,033,296.5 ns |
//| GCAllocTests_NoLohCompact |      1000 |            20000 |     961,671.8 ns |     15,189.58 ns |     12,683.98 ns |     962,678.9 ns |
//|   GCAllocTests_LohCompact |      1000 |            20000 |   1,070,706.9 ns |     18,428.41 ns |     23,306.04 ns |   1,063,153.3 ns |
//|          NativeAllocTests |      1000 |           100000 |   6,067,221.7 ns |    395,076.52 ns |  1,164,892.02 ns |   6,142,965.6 ns |
//| GCAllocTests_NoLohCompact |      1000 |           100000 |  36,131,976.3 ns |    713,677.08 ns |  1,551,475.24 ns |  36,218,566.7 ns |
//|   GCAllocTests_LohCompact |      1000 |           100000 |  36,391,212.2 ns |    723,914.23 ns |  2,017,983.53 ns |  36,760,067.9 ns |
//|          NativeAllocTests |     60000 |               10 |       1,886.9 ns |        162.09 ns |        477.93 ns |       1,945.7 ns |
//| GCAllocTests_NoLohCompact |     60000 |               10 |      70,918.7 ns |      1,336.58 ns |      1,250.24 ns |      71,231.0 ns |
//|   GCAllocTests_LohCompact |     60000 |               10 |      71,556.9 ns |      1,338.86 ns |      1,374.91 ns |      71,845.4 ns |
//|          NativeAllocTests |     60000 |             1000 |   1,314,678.5 ns |     33,397.82 ns |     89,145.53 ns |   1,332,521.1 ns |
//| GCAllocTests_NoLohCompact |     60000 |             1000 |   3,796,132.3 ns |     61,545.98 ns |     65,853.50 ns |   3,792,528.9 ns |
//|   GCAllocTests_LohCompact |     60000 |             1000 |   3,813,853.0 ns |     71,555.35 ns |     73,482.08 ns |   3,800,453.1 ns |
//|          NativeAllocTests |     60000 |            20000 |  77,458,301.9 ns |    818,023.61 ns |    765,179.79 ns |  77,698,771.4 ns |
//| GCAllocTests_NoLohCompact |     60000 |            20000 | 173,141,405.6 ns |  3,447,170.83 ns |  7,638,692.45 ns | 174,112,266.7 ns |
//|   GCAllocTests_LohCompact |     60000 |            20000 | 168,918,652.6 ns |  3,324,550.59 ns |  6,639,486.75 ns | 169,290,150.0 ns |
//|          NativeAllocTests |     60000 |           100000 | 919,239,333.3 ns | 17,820,343.39 ns | 16,669,160.26 ns | 922,794,100.0 ns |
//| GCAllocTests_NoLohCompact |     60000 |           100000 | 890,009,620.8 ns | 21,954,507.53 ns | 63,343,774.03 ns | 894,087,550.0 ns |
//|   GCAllocTests_LohCompact |     60000 |           100000 | 903,627,814.1 ns | 25,248,239.12 ns | 74,048,714.69 ns | 912,168,300.0 ns |
//|          NativeAllocTests |    100000 |               10 |       2,500.8 ns |         86.28 ns |        254.38 ns |       2,381.9 ns |
//| GCAllocTests_NoLohCompact |    100000 |               10 |      56,864.8 ns |        372.58 ns |        330.28 ns |      56,774.7 ns |
//|   GCAllocTests_LohCompact |    100000 |               10 |      79,814.7 ns |      1,427.32 ns |      1,265.28 ns |      80,136.2 ns |
//|          NativeAllocTests |    100000 |             1000 |   2,329,499.4 ns |     20,662.84 ns |     19,328.03 ns |   2,324,207.0 ns |
//| GCAllocTests_NoLohCompact |    100000 |             1000 |   6,002,060.3 ns |    119,459.41 ns |    122,676.02 ns |   5,996,043.0 ns |
//|   GCAllocTests_LohCompact |    100000 |             1000 |   6,199,127.0 ns |    121,488.63 ns |    129,991.47 ns |   6,227,937.9 ns |
//|          NativeAllocTests |    100000 |            20000 |  82,144,818.1 ns |  1,192,526.37 ns |  1,115,489.91 ns |  82,534,585.7 ns |
//| GCAllocTests_NoLohCompact |    100000 |            20000 | 131,358,089.5 ns |  2,544,808.31 ns |  2,828,547.70 ns | 130,981,050.0 ns |
//|   GCAllocTests_LohCompact |    100000 |            20000 | 131,666,960.7 ns |  1,906,222.84 ns |  1,689,816.90 ns | 131,449,900.0 ns |
//|          NativeAllocTests |    100000 |           100000 | 833,641,697.0 ns | 18,841,445.54 ns | 55,554,427.29 ns | 842,961,700.0 ns |
//| GCAllocTests_NoLohCompact |    100000 |           100000 | 678,752,960.7 ns |  7,240,619.17 ns |  6,418,620.30 ns | 677,286,075.0 ns |
//|   GCAllocTests_LohCompact |    100000 |           100000 | 694,618,335.7 ns |  3,477,170.48 ns |  3,082,421.06 ns | 694,361,125.0 ns |