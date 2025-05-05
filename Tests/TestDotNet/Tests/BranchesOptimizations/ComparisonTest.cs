using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

//BenchmarkDotNet = v0.10.1, OS = Microsoft Windows NT 6.1.7601 Service Pack 1
//Processor = Intel(R) Core(TM) i5 - 3210M CPU 2.50GHz, ProcessorCount = 4
//Frequency = 2435917 Hz, Resolution = 410.5230 ns, Timer = TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//DefaultJob: Clr 4.0.30319.42000, 64bit RyuJIT-v4.6.1590.0
//
//	                  Method |       Mean |    StdDev |
//-------------------------- | ---------- | --------- |
// Int_TwoChecks_TwoBranches | 27.6597 ms | 0.1177 ms |
// Int_UnsignedTrick         | 12.2865 ms | 0.0896 ms |




//BenchmarkDotNet=v0.13.1, OS=Windows 10.0.26100
//13th Gen Intel Core i5-13600K, 1 CPU, 20 logical and 14 physical cores
//.NET SDK=9.0.203
//  [Host]     : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//  DefaultJob : .NET 8.0.15 (8.0.1525.16413), X64 RyuJIT
//
//|                      Method |      Mean |     Error |    StdDev |
//|---------------------------- |----------:|----------:|----------:|
//|   Int_TwoChecks_TwoBranches | 19.373 ms | 0.3820 ms | 0.4692 ms |
//|     Int_TwoChecks_OneBranch |  7.122 ms | 0.1412 ms | 0.2651 ms |
//|           Int_UnsignedTrick |  6.206 ms | 0.1225 ms | 0.1361 ms |
//| Float_TwoChecks_TwoBranches | 18.223 ms | 0.1078 ms | 0.1008 ms |
//|   Float_TwoChecks_OneBranch | 11.575 ms | 0.1765 ms | 0.1651 ms |
//|              Float_OneCheck |  6.885 ms | 0.1347 ms | 0.1704 ms |


namespace TestDotNet.Tests.BranchesOptimizations;

public class ComparisonTest
{
    private readonly int[] signedValuesF;
    private readonly uint[] unsignedValuesF;

    const int minValueInArray = -500; // inclusive
    const int maxValueInArray = 2500; // exclusive

    const int minValueFilter = 5; // exclusive
    const int maxValueFilter = 100; // exclusive

    private readonly float[] floatValuesF;

    public ComparisonTest()
    {
        if (minValueFilter >= maxValueFilter || minValueInArray >= maxValueInArray ||
            minValueFilter <= minValueInArray || maxValueFilter >= maxValueInArray)
            throw new Exception("Check bounds.");

        const int count = 10000000;
        signedValuesF = RandomHelper.GetIntNumbers(count, minValueInArray, maxValueInArray);
        unsignedValuesF = SickSh_tApi.Cast<int[], uint[]>(signedValuesF);
        floatValuesF = signedValuesF.Select(x => (float)x).ToArray();
    }

    [Benchmark]
    public int Int_TwoChecks_TwoBranches()
    {
        int counter = 0;
        int[] signedValues = signedValuesF;
        for (int i = 0; i < signedValues.Length; i++)
        {
            int value = signedValues[i];
            if (value > minValueFilter && value < maxValueFilter)
                counter++;
        }
        return counter;
    }

    [Benchmark]
    public int Int_TwoChecks_OneBranch()
    {
        int counter = 0;
        int[] signedValues = signedValuesF;
        for (int i = 0; i < signedValues.Length; i++)
        {
            int value = signedValues[i];
            if (value > minValueFilter & value < maxValueFilter)
                counter++;
        }
        return counter;
    }

    [Benchmark]
    public int Int_UnsignedTrick()
    {
        int counter = 0;
        uint[] unsignedValues = unsignedValuesF;
        const uint delta = minValueFilter + 1;
        const uint newUpperBound = maxValueFilter - delta;

        for (int i = 0; i < unsignedValues.Length; i++)
        {
            uint value = unsignedValues[i] - delta;
            if (value < newUpperBound)
                counter++;
        }
        return counter;
    }

    [Benchmark]
    public int Float_TwoChecks_TwoBranches()
    {
        int counter = 0;
        float[] floatValues = floatValuesF;
        for (int i = 0; i < floatValues.Length; i++)
        {
            float value = floatValues[i];
            if (minValueFilter < value && value < maxValueFilter)
                counter++;
        }
        return counter;
    }

    [Benchmark]
    public int Float_TwoChecks_OneBranch()
    {
        int counter = 0;
        float[] floatValues = floatValuesF;
        for (int i = 0; i < floatValues.Length; i++)
        {
            float value = floatValues[i];
            if (minValueFilter < value & value < maxValueFilter)
                counter++;
        }
        return counter;
    }

    [Benchmark]
    public int Float_OneCheck()
    {
        int counter = 0;
        float[] floatValues = floatValuesF;
        for (int i = 0; i < floatValues.Length; i++)
        {
            float value = floatValues[i];
            if ((value - minValueFilter) * (maxValueFilter - value) > 0)
                counter++;
        }
        return counter;
    }
}
