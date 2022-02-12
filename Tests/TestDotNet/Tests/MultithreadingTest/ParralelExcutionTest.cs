using BenchmarkDotNet.Attributes;
using TestDotNet.Utils;

//	BenchmarkDotNet=v0.10.7, OS=Windows 10.0.17134
//	Processor=Intel Core i7-8700K CPU 3.70GHz, ProcessorCount=12
//	Frequency=3609382 Hz, Resolution=277.0557 ns, Timer=TSC
//[Host]     : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0
//DefaultJob : Clr 4.0.30319.42000, 64bit RyuJIT-v4.7.3110.0


//Method |       Mean |      Error |     StdDev |
//--------------------------- |-----------:|-----------:|-----------:|
//SingleTread | 3,994.9 us | 58.3344 us | 54.5660 us |
//Parralel12OptimizedByCalls |   626.4 us |  4.5140 us |  4.0015 us |
//ParralelDefault |   673.2 us |  1.0565 us |  0.8822 us |
//Parralel12 |   678.2 us |  1.0149 us |  0.7924 us |
//Parralel6 |   943.6 us | 17.4942 us | 16.3641 us |
//Parralel4 |   940.3 us | 18.5411 us | 19.0403 us |
//Parralel14 |   683.7 us |  1.0010 us |  0.7815 us |
//Parralel16 |   685.8 us |  0.8335 us |  0.7796 us |

namespace TestDotNet.Tests.MultithreadingTest;

public class ParralelExcutionTest
{
    private readonly Matrix4F[] inPrams;
    private readonly Matrix4F[] outPrams;
    private const int MatricesCount = 120_000;

    public ParralelExcutionTest()
    {
        inPrams = GetRandomMatrices(MatricesCount);
        outPrams = new Matrix4F[MatricesCount];
    }


    [Benchmark]
    public void SingleTread()
    {
        Matrix4F[] _in = inPrams;
        Matrix4F[] _out = outPrams;
        for (int i = 0; i < inPrams.Length; i++)
        {
            Matrix4F.Invert(_in[i], out _out[i]);
        }
    }

    private int countInBatch;
    [Benchmark]
    public void Parralel12OptimizedByCalls()
    {
        int batches = Environment.ProcessorCount;
        countInBatch = MatricesCount / batches;
        Parallel.For(0, batches, new ParallelOptions()
        {
            MaxDegreeOfParallelism = batches,
        }, InvertStartingFrom);
    }
    private void InvertStartingFrom(int batchIndex)
    {
        Matrix4F[] _in = inPrams;
        Matrix4F[] _res = outPrams;
        int startIndex = batchIndex * countInBatch;
        int stopIndex = Math.Min(startIndex + countInBatch, _in.Length);

        for (int i = startIndex; i < stopIndex; i++)
        {
            Matrix4F.Invert(_in[i], out _res[i]);
        }
    }

    [Benchmark]
    public void ParralelDefault()
    {
        Parallel.For(0, MatricesCount, InvertOne);
    }

    [Benchmark]
    public void Parralel12()
    {
        Parallel.For(0, MatricesCount, new ParallelOptions()
        {
            MaxDegreeOfParallelism = 12
        }, InvertOne);
    }

    [Benchmark]
    public void Parralel6()
    {
        Parallel.For(0, MatricesCount, new ParallelOptions
        {
            MaxDegreeOfParallelism = 6
        }, InvertOne);
    }

    [Benchmark]
    public void Parralel4()
    {
        Parallel.For(0, MatricesCount, new ParallelOptions
        {
            MaxDegreeOfParallelism = 6
        }, InvertOne);
    }

    [Benchmark]
    public void Parralel14()
    {
        Parallel.For(0, MatricesCount, new ParallelOptions
        {
            MaxDegreeOfParallelism = 14
        }, InvertOne);
    }

    [Benchmark]
    public void Parralel16()
    {
        Parallel.For(0, MatricesCount, new ParallelOptions
        {
            MaxDegreeOfParallelism = 14
        }, InvertOne);
    }

    private void InvertOne(int index)
    {
        Matrix4F.Invert(inPrams[index], out outPrams[index]);
    }


    private static Matrix4F[] GetRandomMatrices(int count)
    {
        var matrices = new Matrix4F[count];
        for (int i = 0; i < count; i++)
        {
            matrices[i] = new Matrix4F
            {
                M11 = RandomHelper.GetFloat(),
                M12 = RandomHelper.GetFloat(),
                M13 = RandomHelper.GetFloat(),
                M14 = RandomHelper.GetFloat(),

                M21 = RandomHelper.GetFloat(),
                M22 = RandomHelper.GetFloat(),
                M23 = RandomHelper.GetFloat(),
                M24 = RandomHelper.GetFloat(),

                M31 = RandomHelper.GetFloat(),
                M32 = RandomHelper.GetFloat(),
                M33 = RandomHelper.GetFloat(),
                M34 = RandomHelper.GetFloat(),

                M41 = RandomHelper.GetFloat(),
                M42 = RandomHelper.GetFloat(),
                M43 = RandomHelper.GetFloat(),
                M44 = RandomHelper.GetFloat(),
            };
        }
        return matrices;
    }
}
